using SmartLedger.Modules.Secirity.Clients.Keycloak.Generated;
using SmartLedger.Modules.Security.Clients.Keycloak.Converters;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Mappers;

/// <summary>
/// Mapper for converting Keycloak UserRepresentation to response model.
/// </summary>
public static class UserResponseMapper
{
    /// <summary>
    /// Maps a <see cref="UserRepresentation"/> to a <see cref="UserResponse"/>.
    /// </summary>
    /// <param name="userRepresentation">User representation model.</param>
    /// <param name="userId">User ID.</param>
    /// <returns><see cref="UserResponse"/> containing the mapped user data.</returns>
    public static UserResponse MapToUserResponse(this UserRepresentation userRepresentation, Guid userId) =>
        new(userId,
            userRepresentation.Username!, 
            userRepresentation.FirstName!,
            userRepresentation.LastName!,
            userRepresentation.Email!,
            userRepresentation.EmailVerified!.Value,
            userRepresentation.CreatedTimestamp.HasValue
                ? DateTimeConverter.FromUnixMillisToDateTimeUtc(userRepresentation.CreatedTimestamp.Value)
                : null);
}