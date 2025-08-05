namespace SmartLedger.Modules.Security.Contracts.Responses.Identify;

/// <summary>
/// Represents a response containing authentication tokens.
/// </summary>
/// <param name="AccessToken">Access token.</param>
/// <param name="RefreshToken">Refresh token.</param>
/// <param name="ExpiresIn">Time when access token expires.</param>
public sealed class LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime? ExpiresIn);