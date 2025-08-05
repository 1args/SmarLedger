using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Mappers;
using SmartLedger.Modules.Security.Contracts.Exceptions;
using SmartLedger.Modules.Security.Contracts.Options;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

/// <inheritdoc />
public sealed class KeycloakUserApiClient(
    IHttpClientFactory httpClientFactory,
    IKeycloakGeneratedApiClient keycloakGeneratedApiClient,
    IOptions<KeycloakAuthorizationOptions> keycloakAuthorizationOptions,
    ILogger<KeycloakUserApiClient> logger) : IKeycloakUserApiClient
{
    private readonly KeycloakAuthorizationOptions _keycloakAuthorizationOptions = keycloakAuthorizationOptions.Value;

    /// <inheritdoc />
    public async Task<UserResponse> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving user information for user with ID: {UserId}", userId);

        var userRepresentation = await GetKeycloakUserAsync(userId, cancellationToken);

        if (userRepresentation is null)
        {
            logger.LogWarning("User with ID {UserId} not found for retrieving user information.", userId);
            throw new KeycloakApiException($"User with ID {userId} not found.");
        }

        logger.LogInformation("Successfully retrieved user information for user with ID {UserId}", userId);

        return userRepresentation.MapToUserResponse(userId);
    }

    /// <inheritdoc />
    public async Task EmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying email for user with ID {UserId} and email {Email}", userId, email);

        var userRepresentation = await GetKeycloakUserAsync(userId, cancellationToken);

        if (userRepresentation is null)
        {
            logger.LogWarning("User with ID {UserId} not found for email verification", userId);
            throw new KeycloakApiException($"User with ID {userId} not found.");
        }

        userRepresentation.EmailVerified = email == userRepresentation.Email;
        await UpdateUserAsync(userRepresentation, cancellationToken);

        logger.LogInformation("Email verification completed for user with ID {UserId} and email {Email}", userId, email);
    }

    /// <summary>
    /// Retrieves user information from Keycloak by user ID.
    /// </summary>
    private async Task<UserRepresentation> GetKeycloakUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var userRepresentation = await keycloakGeneratedApiClient.UsersGET2Async(
                _keycloakAuthorizationOptions.Realm,
                userId.ToString(),
                cancellationToken: cancellationToken);

            return userRepresentation;

        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to retrieve user information for user with ID";
            logger.LogError(ex, errorMessage + " {UserId}", userId);
            throw new KeycloakApiException($"{errorMessage} {userId}.");
        }
    }

    /// <summary>
    /// Updates user information in Keycloak.
    /// </summary>
    private async Task UpdateUserAsync(UserRepresentation userRepresentation, CancellationToken cancellationToken)
    {
        try
        {
            await keycloakGeneratedApiClient.UsersPUTAsync(
                _keycloakAuthorizationOptions.Realm,
                userRepresentation.Id!.ToString(),
                userRepresentation,
                cancellationToken);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to update user information for user with ID";
            var data = new { userRepresentation.Email, userRepresentation.EmailVerified };

            logger.LogError(
                ex,
                errorMessage + " {UserId} to new data: {@Data}",
                userRepresentation.Id,
                JsonSerializer.Serialize(data));
            throw new KeycloakApiException($"{errorMessage} {userRepresentation.Id}.");
        }
    }
}