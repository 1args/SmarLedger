using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;

/// <summary>
/// Represents a query to refresh a user's access token.
/// </summary>
/// <param name="RefreshToken">Refresh token.</param>
public sealed record RefreshTokenQuery(
    string RefreshToken) : IQuery<LoginResponse>;