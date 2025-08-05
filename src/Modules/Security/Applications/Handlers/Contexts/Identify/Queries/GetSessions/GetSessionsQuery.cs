using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Queries.GetSessions;

/// <summary>
/// Represents a query to retrieve a list of user sessions.
/// </summary>
public sealed record GetSessionsQuery : IQuery<IReadOnlyCollection<UserSessionResponse>>;