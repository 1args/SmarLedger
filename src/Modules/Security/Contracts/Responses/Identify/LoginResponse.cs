namespace SmartLedger.Modules.Security.Contracts.Responses.Identify;

/// <summary>
/// Represents a response containing authentication tokens.
/// </summary>
/// <param name="AccessToken">Access token.</param>
/// <param name="ExpiresInMinutes">Time when access token expires.</param>
/// <param name="RefreshToken">Refresh token.</param>
/// <param name="TokenType">Refresh token.</param>
public sealed record LoginResponse(
    string AccessToken,
    long? ExpiresInMinutes,
    string RefreshToken,
    string TokenType);