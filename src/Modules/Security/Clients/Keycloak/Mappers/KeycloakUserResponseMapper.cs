using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Models;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Mappers;

/// <summary>
/// Mapper for converting Keycloak UserRepresentation to response model.
/// </summary>
public static class KeycloakUserResponseMapper
{
    /// <summary>
    /// Maps a <see cref="UserRepresentation"/> to a <see cref="KeycloakUserResponse"/>.
    /// </summary>
    /// <param name="userRepresentation">User representation model.</param>
    /// <returns><see cref="KeycloakUserResponse"/> containing the mapped user data.</returns>
    public static KeycloakUserResponse MapToKeycloakUserResponse(this UserRepresentation userRepresentation, Guid userId) =>
        new(userId,
            userRepresentation.Username!, 
            userRepresentation.FirstName!,
            userRepresentation.LastName!,
            userRepresentation.Email!,
            userRepresentation.EmailVerified!.Value,
            userRepresentation.CreatedTimestamp.HasValue
                ? DateTimeOffset.FromUnixTimeMilliseconds(userRepresentation.CreatedTimestamp.Value).UtcDateTime
                : null);
}