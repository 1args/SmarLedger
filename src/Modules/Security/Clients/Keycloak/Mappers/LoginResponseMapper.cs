using SmartLedger.Modules.Security.Clients.Keycloak.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Mappers;

/// <summary>
/// Mapper for converting token response to response model.
/// </summary>
public static class LoginResponseMapper
{
    /// <summary>
    /// Maps a <see cref="TokenResponse"/> to a <see cref="LoginResponse"/>.
    /// </summary>
    /// <param name="tokenResponse">Token response.</param>
    /// <returns><see cref="LoginResponse"/> containing the mapped login data.</returns>
    public static LoginResponse MapToLoginResponse(this TokenResponse tokenResponse) =>
        new(tokenResponse.AccessToken,
            tokenResponse.ExpiresIn!.Value / 60,
            tokenResponse.RefreshToken,
            tokenResponse.TokenType);
}