using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users;

/// <summary>
/// Service for managing user-related operations.
/// </summary>
public sealed class UsersService(
    IKeycloakUserApiClient keycloakUserApiClient,
    Lazy<IAuthorizationData> authorizationData,
    ILogger<UsersService> logger) : IUsersService
{
    /// <inheritdoc />
    public async Task<UserResponse> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;
        return await keycloakUserApiClient.GetUserAsync(userId, cancellationToken);
    }
}