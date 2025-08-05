using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;

/// <summary>
/// Interface for handling user authorization.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Authorizes a user with the provided login data.
    /// </summary>
    /// <param name="request">Model containing username and password.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing tokens.</returns>
    Task<LoginResponse> AuthorizeAsync(LoginModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <param name="request">Model containing refresh token.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing tokens.</returns>
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenOnlyModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Logs out the user by invalidating their sessions.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LogoutAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the list of active user sessions.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of active user sessions.</returns>
    Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Resets the password for a user.
    /// </summary>
    /// <param name="request">Model containing current and new passwords.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ResetPasswordAsync(ResetPasswordModel request, CancellationToken cancellationToken);
}