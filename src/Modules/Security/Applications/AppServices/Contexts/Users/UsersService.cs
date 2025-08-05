using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Models;
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
        if (!authorizationData.Value.UserId.HasValue)
        {
            logger.LogWarning("User ID is not available for retrieving current user information.");
            throw new UnauthorizedAccessException("User ID is required to retrieve current user information.");
        }

        var userId = authorizationData.Value.UserId.Value;
        return await keycloakUserApiClient.GetUserAsync(userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task EmailVerificationAsync(EmailOnlyModel request, CancellationToken cancellationToken)
    {
        if (!authorizationData.Value.UserId.HasValue)
        {
            logger.LogWarning("User ID is not available for email verification.");
            throw new UnauthorizedAccessException("User ID is required to verify email.");
        }
        var userId = authorizationData.Value.UserId.Value;
        await keycloakUserApiClient.EmailVerificationAsync(userId, request.Email, cancellationToken);
    }
}