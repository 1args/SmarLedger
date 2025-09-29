using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

/// <summary>
/// Keycloak user API Client.
/// </summary>
public interface IKeycloakUserApiClient
{
    /// <summary>
    /// Retrieves user information from Keycloak by user ID.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing user information.</returns>
    Task<UserResponse> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}