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
        logger.LogInformation(
            "Ininiating user registration for user with username {Username} and email {Email}",
            request.Username, request.Email);

        var userCreationModel = new UserCreationModel
        (
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );

        var userId = await keycloakAuthorizationApiClient.CreateUserAsync(userCreationModel, cancellationToken);
        await keycloakAuthorizationApiClient.SendVerificationEmailAsync(userId, cancellationToken);

        logger.LogInformation(
            "Registration successfully completed for user with username {Username} and ID {UserId}", 
            request.Username, userId);
    }

    /// <inheritdoc />
    public async Task<LoginResponse> AuthorizeAsync(LoginModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Initiating an authorization attempt for a username {Username}", request.Username);

        var tokenResponse = await keycloakAuthorizationApiClient.AuthorizeAsync(
            request.Username,
            request.Password, 
            cancellationToken);

        logger.LogInformation("User with username {Username} successfully authorized", request.Username);

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

        logger.LogInformation("Initiating logout from all sessions for user with ID {UserId}", userId);

        await keycloakAuthorizationApiClient.LogoutAsync(userId, cancellationToken);

        logger.LogInformation("Successfully logged out of all sessions for user with ID {UserId}", userId);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserSessionResponse>> GetUserSessionsAsync(CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        return await keycloakAuthorizationApiClient.GetUserSessionsAsync(userId, cancellationToken);
    }
}