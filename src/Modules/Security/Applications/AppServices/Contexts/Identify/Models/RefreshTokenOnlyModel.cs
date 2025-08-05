namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;

/// <summary>
/// Represents a model containing only the refresh token.
/// </summary>
/// <param name="RefreshToken">Refresh token.</param>
public sealed record RefreshTokenOnlyModel(string RefreshToken);