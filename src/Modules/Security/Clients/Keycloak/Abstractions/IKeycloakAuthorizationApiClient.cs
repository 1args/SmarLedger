using SmartLedger.Modules.Security.Contracts.Responses;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

/// <summary>
/// Keycloak authorization API Client.
/// </summary>
public interface IKeycloakAuthorizationApiClient
{
    /// <summary>
    /// Authorizes a user with the provided username and password.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <param name="password">User password.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing tokens.</returns>
    Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes the access token using the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing tokens.</returns>
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Logs out a user by invalidating their session in Keycloak.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken);
}