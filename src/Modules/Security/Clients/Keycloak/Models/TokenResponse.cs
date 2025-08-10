using System.Text.Json.Serialization;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents a response containing access and refresh tokens from Keycloak.
/// </summary>
public sealed record TokenResponse
{
    /// <summary>Access token issued by Keycloak.</summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>Time in seconds until the access token expires.s</summary>
    [JsonPropertyName("expires_in")]
    public long? ExpiresIn { get; init; }

    /// <summary>Refresh token issued by Keycloak, used to obtain a new access token without re-authentication.</summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>Type of the token, typically "Bearer".</summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = string.Empty;
};