using System.Net;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Helpers;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Mappers;
using SmartLedger.Modules.Security.Clients.Keycloak.Models;
using SmartLedger.Modules.Security.Contracts.Exceptions;
using SmartLedger.Modules.Security.Contracts.Options;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

/// <inheritdoc />
public sealed class KeycloakAuthorizationApiClient(
    IHttpClientFactory httpClientFactory,
    IKeycloakGeneratedApiClient keycloakGeneratedApiClient,
    IOptions<KeycloakAuthorizationOptions> keycloakAuthorizationOptions,
    ILogger<KeycloakAuthorizationApiClient> logger) : IKeycloakAuthorizationApiClient
{
    private readonly KeycloakAuthorizationOptions _keycloakAuthorizationOptions = keycloakAuthorizationOptions.Value;

    /// <summary>Message indicating that the authorization server failed to provide necessary information.</summary>
    private const string AuthorizationServerFailedMessage = "Failed to get the necessary information from the authorization server.";

    /// <summary>Default request timeout (in seconds) for HTTP calls to Keycloak.</summary>
    private const int DefaultTimeoutSeconds = 30;

    /// <summary>Timeout (in seconds) for token-related HTTP requests.</summary>
    private const int TokenTimeoutSeconds = 10;

    /// <summary>Lifespan (in seconds) of the verification email link sent by Keycloak.</summary>
    private const int VerificationEmailLifespan = 60 * 5;

    /// <inheritdoc />
    public async Task<Guid> CreateUserAsync(UserCreationModel request, CancellationToken cancellationToken)
    {
        try
        {
            var userRepresentation = new UserRepresentation
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Enabled = true,
                EmailVerified = false,
                Credentials = new List<CredentialRepresentation>
                {
                    new()
                    {
                        Type = "password",
                        Value = request.Password,
                        Temporary = false
                    }
                }
            }; 

            await keycloakGeneratedApiClient.UsersPOSTAsync(
                _keycloakAuthorizationOptions.Realm,
                userRepresentation,
                cancellationToken: cancellationToken);

            return await GetUserIdByUsername(request.Username, cancellationToken);
        }
        catch (KeycloakGeneratedApiException ex) when (ex.StatusCode == 409)
        {
            logger.LogWarning("User creation failed: {ErrorMessage}", ex.Response);
            throw new ConflictException("User with that name or email address already exists.");
        }
        catch (KeycloakGeneratedApiException ex) when (ex.StatusCode == 400)
        {
            logger.LogWarning("Validation error: {ErrorMessage}", ex.Response);
            throw new KeycloakApiException("Registration data is invalid. Please check all fields.");
        }
        catch (KeycloakGeneratedApiException ex)
        {
            logger.LogError(ex, "Creation failed for user {Username}", request.Username);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task SendVerificationEmailAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            await keycloakGeneratedApiClient.SendVerifyEmailAsync(
                _keycloakAuthorizationOptions.Realm,
                userId.ToString(),
                client_id: _keycloakAuthorizationOptions.ClientId,
                lifespan: VerificationEmailLifespan,
                cancellationToken: cancellationToken);
        }
        catch (KeycloakGeneratedApiException ex)
        {
            logger.LogError(ex, "Failed to send verification email for user {UserId}", userId);
            throw new KeycloakApiException("Failed to send verification email. Please try again.");
        }
    }

    /// <inheritdoc />
    public async Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();

            var requestBody = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "grant_type", "password" },
                { "client_id", _keycloakAuthorizationOptions.ClientId },
                { "client_secret", _keycloakAuthorizationOptions.ClientSecret },
            };

            var discoveryDocument = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
            ValidateTokenEndpoint(discoveryDocument.TokenEndpoint);

            var response = await SendTokenRequestAsync(
                httpClient,
                discoveryDocument.TokenEndpoint,
                requestBody,
                username,
                cancellationToken);

            return response;
        }
        catch (KeycloakApiException ex)
        {
            logger.LogError(ex, "Authorization failed for user {Username}", username);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during authorization for user {Username}", username);
            throw new KeycloakApiException("An error occurred during authorization. Please try again.");
        }
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();

            var requestBody = new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" },
                { "client_id", _keycloakAuthorizationOptions.ClientId },
                { "client_secret", _keycloakAuthorizationOptions.ClientSecret },
            };

            var discoveryDocument = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);
            ValidateTokenEndpoint(discoveryDocument.TokenEndpoint);

            var response = await SendTokenRequestAsync(
                httpClient,
                discoveryDocument.TokenEndpoint,
                requestBody,
                null,
                cancellationToken);

            return response;
        }
        catch (KeycloakApiException ex)
        {
            logger.LogError(ex, "Token refresh failed");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during token refresh");
            throw new KeycloakApiException("An error occurred while updating the token. Please try again.");
        }
    }

    /// <inheritdoc />
    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            await keycloakGeneratedApiClient.LogoutAsync(
                _keycloakAuthorizationOptions.Realm,
                userId.ToString(),
                cancellationToken);
        }
        catch (KeycloakGeneratedApiException ex)
        {
            logger.LogError(ex, "Unexpected error during logout for user with ID {UserId}", userId);
            throw new KeycloakApiException("Failed to log out. Please try again.");
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await keycloakGeneratedApiClient.SessionsAllAsync(
                _keycloakAuthorizationOptions.Realm,
                userId.ToString(),
                cancellationToken);

            var userSessions = sessions
                .Select(s => s.MapToUserSessionResponse())
                .ToList();

            return userSessions.AsReadOnly();
        }
        catch (KeycloakGeneratedApiException ex)
        {
            logger.LogError(ex, "Failed to retrieve sessions for user {UserId}", userId);
            throw new KeycloakApiException("The list of user sessions could not be retrieved. Please try again.");
        }
    }

    /// <summary>
    /// Retrieves the Keycloak user ID for the specified username.
    /// </summary>
    private async Task<Guid> GetUserIdByUsername(string username, CancellationToken cancellationToken)
    {
        try
        {
            var users = await keycloakGeneratedApiClient.UsersAll3Async(
                _keycloakAuthorizationOptions.Realm,
                username: username,
                cancellationToken: cancellationToken);

            var user = users.FirstOrDefault();

            if (user is null || string.IsNullOrWhiteSpace(user.Id))
            {
                throw new KeycloakApiException($"Failed to retrieve created user ID for {username}");
            }

            return GuidHelper.ConvertFromStringToGuid(user.Id);
        }
        catch (KeycloakGeneratedApiException ex)
        {
            logger.LogError(ex, "Failed to get user ID for {Username}", username);
            throw new KeycloakApiException($"Failed to retrieve user ID: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the Keycloak discovery document containing metadata.
    /// </summary>
    private async Task<DiscoveryDocument> GetDiscoveryDocumentAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var discovery = await _keycloakAuthorizationOptions.MetadataAddress
            .WithTimeout(TimeSpan.FromSeconds(DefaultTimeoutSeconds))
            .GetJsonAsync<DiscoveryDocument>(cancellationToken: cancellationToken);

        return discovery;
    }

    /// <summary>
    /// Sends a token request to the Keycloak server.
    /// </summary>
    private async Task<TokenResponse> SendTokenRequestAsync(
        HttpClient httpClient,
        string tokenEndpoint,
        Dictionary<string, string> requestBody,
        string? identifier,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await tokenEndpoint
                .WithTimeout(TokenTimeoutSeconds)
                .PostUrlEncodedAsync(requestBody, cancellationToken: cancellationToken)
                .ReceiveJson<TokenResponse>();

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                logger.LogWarning("Empty access token received for {Identifier}", identifier ?? "refresh-token");
                throw new KeycloakApiException("Failed to retrieve access token. Please try again.");
            }

            return response;
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            logger.LogError(ex, "Invalid credentials provided for {Identifier}", identifier ?? "refresh-token");
            throw new KeycloakApiException(
                "Invalid login or password entered. Please check your credentials.");
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            var errorMessage = await ex.GetResponseStringAsync();
            if (errorMessage.Contains("Account is not fully set up"))
            {
                logger.LogWarning("Authorization failed for {Identifier}. Email not verified", identifier ?? "unknown");
                throw new KeycloakApiException("Email not verified. Please verify your email before logging in.");
            }
            logger.LogError(ex, "An error occurred while executing a token request: {ErrorMessage}", errorMessage);
            throw new KeycloakApiException(AuthorizationServerFailedMessage);
        }
        catch (FlurlHttpException ex)
        {
            var errorMessage = await ex.GetResponseStringAsync();
            logger.LogError(ex, "An error occurred while executing a token request: {ErrorMessage}", errorMessage);
            throw new KeycloakApiException(AuthorizationServerFailedMessage);
        }
    }

    /// <summary>
    /// Validates the token endpoint from the discovery document.
    /// </summary>
    private void ValidateTokenEndpoint(string? tokenEndpoint)
    {
        if (string.IsNullOrWhiteSpace(tokenEndpoint))
        {
            logger.LogWarning("Token endpoint is missing or invalid for authorization request");
            throw new KeycloakApiException(AuthorizationServerFailedMessage);
        }
    }
}