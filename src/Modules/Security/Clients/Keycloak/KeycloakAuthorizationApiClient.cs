using System.Net;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Models;
using SmartLedger.Modules.Security.Contracts.Exceptions;
using SmartLedger.Modules.Security.Contracts.Options;
using SmartLedger.Modules.Security.Contracts.Responses;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

/// <summary>
/// Keycloak authorization API Client.
/// </summary>
public sealed class KeycloakAuthorizationApiClient(
    IHttpClientFactory httpClientFactory,
    IKeycloakGeneratedApiClient keycloakGeneratedApiClient,
    IOptions<KeycloakAuthorizationOptions> keycloakAuthorizationOptions,
    ILogger<KeycloakAuthorizationApiClient> logger) : IKeycloakAuthorizationApiClient
{
    private readonly KeycloakAuthorizationOptions _keycloakAuthorizationOptions = keycloakAuthorizationOptions.Value;

    private const string AuthorizationServerFailedMessage = "Failed to get the necessary information from the authorization server.";

    /// <inheritdoc />
    public async Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initiating authorization for user {Username}", username);

        try
        {
            using var httpClient = httpClientFactory.CreateClient();

            var requestBody = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "grant_type", "password" },
                { "clientId", _keycloakAuthorizationOptions.ClientId },
                { "client_secret", _keycloakAuthorizationOptions.ClientSecret },
            };

            var discoveryDocument = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);

            if (discoveryDocument is null || string.IsNullOrWhiteSpace(discoveryDocument.TokenEndpoint))
            {
                logger.LogError("Token endpoint is missing or invalid for authorization request");
                throw new KeycloakApiException(AuthorizationServerFailedMessage);
            }

            var response = await SendTokenRequestAsync(
                httpClient,
                discoveryDocument.TokenEndpoint,
                requestBody,
                username,
                cancellationToken);

            logger.LogInformation("User {Username} successfully authorized", username);

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
        logger.LogInformation("Initiating token refresh");

        try
        {
            using var httpClient = httpClientFactory.CreateClient();

            var requestBody = new Dictionary<string, string>
            {
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" },
                { "clientId", _keycloakAuthorizationOptions.ClientId },
                { "client_secret", _keycloakAuthorizationOptions.ClientSecret },
            };

            var discoveryDocument = await GetDiscoveryDocumentAsync(httpClient, cancellationToken);

            if (discoveryDocument is null || string.IsNullOrWhiteSpace(discoveryDocument.TokenEndpoint))
            {
                logger.LogError("Token endpoint is missing or invalid for authorization request");
                throw new KeycloakApiException("Failed to get the necessary information from the authorization server.");
            }

            var response = await SendTokenRequestAsync(
                httpClient,
                discoveryDocument.TokenEndpoint,
                requestBody,
                null,
                cancellationToken);

            logger.LogInformation("Token successfully refreshed");

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
        logger.LogInformation("Initiating logout for user with ID {UserId}", userId);

        try
        {
            await keycloakGeneratedApiClient.LogoutAsync(
                _keycloakAuthorizationOptions.Realm,
                userId.ToString(),
                cancellationToken);

            logger.LogInformation("Successfully logged out user with ID {UserId}", userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during logout for user with ID {UserId}", userId);
            throw new KeycloakApiException("Failed to log out. Please try again.");
        }
    }

    /// <summary>
    /// Retrieves the Keycloak discovery document containing metadata.
    /// </summary>
    private async Task<DiscoveryDocument> GetDiscoveryDocumentAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var discovery = await _keycloakAuthorizationOptions.MetadataAddress
            .WithTimeout(TimeSpan.FromSeconds(30))
            .GetJsonAsync<DiscoveryDocument>(cancellationToken: cancellationToken);

        logger.LogInformation(
            "Successfully retrieved discovery document from {MetadataAddress}",
            _keycloakAuthorizationOptions.MetadataAddress);

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
                .WithTimeout(TimeSpan.FromSeconds(10))
                .PostUrlEncodedAsync(requestBody, cancellationToken: cancellationToken)
                .ReceiveJson<TokenResponse>();

            if (string.IsNullOrWhiteSpace(response.AccessToken))
            {
                logger.LogError("Empty access token received for {Identifier}", identifier ?? "refresh-token");
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
        catch (FlurlHttpException ex)
        {
            var errorMessage = await ex.GetResponseStringAsync();
            logger.LogError(ex, "An error occurred while executing a token request: {ErrorMessage}", errorMessage);
            throw new KeycloakApiException(AuthorizationServerFailedMessage);
        }
    }
}