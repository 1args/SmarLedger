namespace SmartLedger.Modules.Security.Contracts.Responses;

public sealed record TokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);