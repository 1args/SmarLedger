namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents a response containing access and refresh tokens from Keycloak.
/// </summary>
/// <param name="AccessToken">Access token.</param>
/// <param name="RefreshToken">Refresh token.</param>
/// <param name="ExpiresIn">Time when access token expires.</param>
public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);