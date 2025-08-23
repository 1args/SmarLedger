using SmartLedger.Common.Contracts.Helpers;
using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
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
        new(GuidHelper.ConvertFromStringToGuid(userSessionRepresentation.Id!), 
            userSessionRepresentation.Start.HasValue 
                ? DateTimeHelper.ConvertFromUnixMillisToDateTimeUtc(userSessionRepresentation.Start!.Value)
                : null,
            userSessionRepresentation.LastAccess.HasValue
                ? DateTimeHelper.ConvertFromUnixMillisToDateTimeUtc(userSessionRepresentation.LastAccess!.Value)
                : null,
            userSessionRepresentation.IpAddress!);
}