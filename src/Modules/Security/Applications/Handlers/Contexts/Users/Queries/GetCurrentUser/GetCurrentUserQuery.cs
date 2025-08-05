using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Users.Queries.GetCurrentUser;

/// <summary>
/// Represents a query to retrieve the current user's information.
/// </summary>
public sealed record GetCurrentUserQuery : IQuery<UserResponse>;