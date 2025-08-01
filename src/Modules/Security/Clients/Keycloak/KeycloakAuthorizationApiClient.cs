using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

internal class KeycloakAuthorizationApiClient : IKeycloakAuthorizationApiClient
{
    public Task<TokenResponse> AuthorizeAsync(string username, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}