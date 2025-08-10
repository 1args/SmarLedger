using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Mappers;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify;

/// <summary>
/// Service for handling user authorization.
/// </summary>
public sealed class AuthorizationService(
    IKeycloakAuthorizationApiClient keycloakAuthorizationApiClient,
    Lazy<IAuthorizationData> authorizationData,
    ILogger<AuthorizationService> logger) : IAuthorizationService
{
    /// <inheritdoc />
    public async Task<LoginResponse> AuthorizeAsync(LoginModel request, CancellationToken cancellationToken)
    {
        var tokenResponse = await keycloakAuthorizationApiClient.AuthorizeAsync(
            request.Username,
            request.Password, 
            cancellationToken);

        return tokenResponse.MapToLoginResponse();
    }

    /// <inheritdoc />
    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenOnlyModel request, CancellationToken cancellationToken)
    {
        var tokenResponse = await keycloakAuthorizationApiClient.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        return tokenResponse.MapToLoginResponse();
    }

    /// <inheritdoc />
    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        if (!userId.HasValue)
        {
            const string errorMessage = "User ID is not available for logout operation";
            logger.LogWarning(errorMessage);
            throw new AuthorizationException($"{errorMessage}.");
        }

        await keycloakAuthorizationApiClient.LogoutAsync(userId.Value, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        if (userId.HasValue)
        {
            return await keycloakAuthorizationApiClient.GetUserSessionsAsync(userId.Value, cancellationToken);
        }

        logger.LogWarning("User ID is not available for retrieving user sessions.");
        return new List<UserSessionResponse>();
    }

    /// <inheritdoc />
    public async Task ResetPasswordAsync(ResetPasswordModel request, CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        if (userId.HasValue)
        {
            await keycloakAuthorizationApiClient.ResetPasswordAsync(
                userId.Value,
                request.CurrentPassword, 
                request.NewPassword,
                cancellationToken);
        }
    }
}