using Microsoft.Extensions.Options;
using SmartLedger.Modules.Security.Contracts.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using Flurl.Http;

namespace SmartLedger.Modules.Security.Clients.Keycloak;

/// <summary>
/// Handler for Keycloak admin authentication.
/// </summary>
public sealed class KeycloakAdminAuthHandler(
    IHttpClientFactory httpClientFactory,
    IOptions<KeycloakAuthorizationOptions> options) : DelegatingHandler
{
    private readonly KeycloakAuthorizationOptions _keycloakAuthorizationOptions = options.Value;

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetAdminTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Retrieves an admin token from Keycloak using client credentials grant type.
    /// </summary>
    private async Task<string> GetAdminTokenAsync(CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var requestBody = new Dictionary<string, string>
        {
            { "client_id", _keycloakAuthorizationOptions.AdminClient },
            { "client_secret", _keycloakAuthorizationOptions.AdminSecret },
            { "grant_type", "client_credentials" }
        };

        var adminTokenUrl = $"{_keycloakAuthorizationOptions.AdminBaseUrl}/realms/{_keycloakAuthorizationOptions.AdminRealm}/protocol/openid-connect/token";

        var response = await adminTokenUrl
            .WithTimeout(TimeSpan.FromSeconds(10))
            .PostUrlEncodedAsync(requestBody, cancellationToken: cancellationToken);

        var content = await response.GetStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(content);

        return tokenResponse.GetProperty("access_token").GetString()!;
    }
}