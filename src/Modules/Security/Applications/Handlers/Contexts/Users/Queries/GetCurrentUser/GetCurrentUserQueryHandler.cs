using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Users;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Users.Queries.GetCurrentUser;

/// <summary>
/// Handles the logic for processing <see cref="GetCurrentUserQuery"/>.
/// </summary>
public sealed class GetCurrentUserQueryHandler(
    IUsersService usersService) : IQueryHandler<GetCurrentUserQuery, UserResponse>
{
    /// <inheritdoc />
    public async Task<UserResponse> HandleAsync(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        return await usersService.GetCurrentUserAsync(cancellationToken);
    }
}