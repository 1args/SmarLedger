using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Converters;
using SmartLedger.Modules.Security.Clients.Keycloak.Models;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Mappers;

/// <summary>
/// Mapper for converting Keycloak UserSessionRepresentation to response model.
/// </summary>
public static class KeycloakUserSessionMapper
{
    /// <summary>
    /// Maps a <see cref="UserRepresentation"/> to a <see cref="KeycloakUserResponse"/>.
    /// </summary>
    /// <param name="userSessionRepresentation">User session representation model.</param>
    /// <returns><see cref="KeycloakUserSessionResponse"/> containing the mapped session data.</returns>
    public static KeycloakUserSessionResponse MapToKeycloakUserSessionResponse(this UserSessionRepresentation userSessionRepresentation) =>
        new(GuidConverter.FromStringToGuid(userSessionRepresentation.Id!), 
            userSessionRepresentation.Start.HasValue 
                ? DateTimeConverter.FromUnixMillisToDateTimeUtc(userSessionRepresentation.LastAccess!.Value)
                : null,
            userSessionRepresentation.LastAccess.HasValue
                ? DateTimeConverter.FromUnixMillisToDateTimeUtc(userSessionRepresentation.LastAccess!.Value)
                : null,
            userSessionRepresentation.IpAddress!);
}