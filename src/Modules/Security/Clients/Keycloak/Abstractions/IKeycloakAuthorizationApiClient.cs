using SmartLedger.Modules.Security.Clients.Keycloak.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

/// <summary>
/// Keycloak authorization API Client.
/// </summary>
public interface IKeycloakAuthorizationApiClient
{
    /// <summary>
    /// Creates a new user in Keycloak with the provided details.
    /// </summary>
    /// <param name="request">Model containing registration data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task<Guid> CreateUserAsync(UserCreationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a verification email to the user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendVerificationEmailAsync(Guid userId, CancellationToken cancellationToken);

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
    /// Logs out a user by invalidating their sessions in Keycloak.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the list of active user sessions for a specific user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of active user sessions.</returns>
    Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(Guid userId, CancellationToken cancellationToken);
}