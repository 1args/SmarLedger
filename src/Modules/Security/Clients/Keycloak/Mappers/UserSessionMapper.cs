using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Converters;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Mappers;

/// <summary>
/// Mapper for converting Keycloak UserSessionRepresentation to response model.
/// </summary>
public static class UserSessionMapper
{
    /// <summary>
    /// Maps a <see cref="UserRepresentation"/> to a <see cref="UserResponse"/>.
    /// </summary>
    /// <param name="userSessionRepresentation">User session representation model.</param>
    /// <returns><see cref="UserSessionResponse"/> containing the mapped session data.</returns>
    public static UserSessionResponse MapToUserSessionResponse(this UserSessionRepresentation userSessionRepresentation) =>
        new(GuidConverter.FromStringToGuid(userSessionRepresentation.Id!), 
            userSessionRepresentation.Start.HasValue 
                ? DateTimeConverter.FromUnixMillisToDateTimeUtc(userSessionRepresentation.Start!.Value)
                : null,
            userSessionRepresentation.LastAccess.HasValue
                ? DateTimeConverter.FromUnixMillisToDateTimeUtc(userSessionRepresentation.LastAccess!.Value)
                : null,
            userSessionRepresentation.IpAddress!);
}