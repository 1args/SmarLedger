using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.GetSessions;

/// <summary>
/// Handles the logic for processing <see cref="GetSessionsQuery"/>.
/// </summary>
public sealed class GetSessionsQueryHandler(
    IAuthorizationService authorizationService) : IQueryHandler<GetSessionsQuery, IReadOnlyCollection<UserSessionResponse>>
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<UserSessionResponse>> HandleAsync(GetSessionsQuery query, CancellationToken cancellationToken)
    {
       return await authorizationService.GetUserSessionsAsync(cancellationToken);
    }
}