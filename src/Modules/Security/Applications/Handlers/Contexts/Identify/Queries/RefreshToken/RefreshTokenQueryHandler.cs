using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.RefreshToken;

/// <summary>
/// Handles the logic for processing <see cref="RefreshTokenQuery"/>.
/// </summary>
public sealed class RefreshTokenQueryHandler(
    IAuthorizationService authorizationService) : IQueryHandler<RefreshTokenQuery, LoginResponse>
{
    /// <inheritdoc />
    public async Task<LoginResponse> HandleAsync(RefreshTokenQuery query, CancellationToken cancellationToken)
    {
        var request = new RefreshTokenOnlyModel(query.RefreshToken);
        return await authorizationService.RefreshTokenAsync(request, cancellationToken);
    }
}