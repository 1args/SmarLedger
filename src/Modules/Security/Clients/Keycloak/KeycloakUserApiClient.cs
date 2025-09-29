using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Mappers;
using SmartLedger.Modules.Security.Contracts.Exceptions;
using SmartLedger.Modules.Security.Contracts.Options;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

/// <inheritdoc />
public sealed class KeycloakUserApiClient(
    IKeycloakGeneratedApiClient keycloakGeneratedApiClient,
    IOptions<KeycloakAuthorizationOptions> keycloakAuthorizationOptions,
    ILogger<KeycloakUserApiClient> logger) : IKeycloakUserApiClient
{
    private readonly KeycloakAuthorizationOptions _keycloakAuthorizationOptions = keycloakAuthorizationOptions.Value;

    /// <inheritdoc />
    public async Task<UserResponse> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userRepresentation = await GetKeycloakUserAsync(userId, cancellationToken);
        return userRepresentation.MapToUserResponse(userId);
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
                cancellationToken: cancellationToken) 
                ?? throw new NotFoundException($"User with ID {userId} not found.");

            return userRepresentation;

        }
        catch (KeycloakGeneratedApiException ex)
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
        catch (KeycloakGeneratedApiException ex)
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