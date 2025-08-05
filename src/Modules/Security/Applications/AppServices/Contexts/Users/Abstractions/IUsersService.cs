using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Abstractions;

/// <summary>
/// Interface for managing user-related operations.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Retrieves the current user information.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing </returns>
    Task<UserResponse> GetCurrentUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Confirms the email address of the current user.
    /// </summary>
    /// <param name="request">Model containing user email.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task EmailVerificationAsync(EmailOnlyModel request, CancellationToken cancellationToken);
}