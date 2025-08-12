using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Clients.Keycloak.Mappers;
using SmartLedger.Modules.Security.Clients.Keycloak.Models;
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
    public async Task RegisterAsync(UserRegistrationModel request, CancellationToken cancellationToken)
    {
        var userCreationModel = new UserCreationModel
        (
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );

        await keycloakAuthorizationApiClient.CreateUserAsync(userCreationModel, cancellationToken);
    }

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

        await keycloakAuthorizationApiClient.LogoutAsync(userId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        return await keycloakAuthorizationApiClient.GetUserSessionsAsync(userId, cancellationToken);
    }
}