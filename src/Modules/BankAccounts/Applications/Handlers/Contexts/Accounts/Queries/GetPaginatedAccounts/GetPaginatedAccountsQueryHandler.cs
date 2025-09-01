using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedAccounts;

/// <summary>
/// Handles the logic for processing <see cref="GetPaginatedAccountsQuery"/>.
/// </summary>
public sealed class GetPaginatedAccountsQueryHandler(
    IAccountsRetrievalService accountsRetrievalService) : IQueryHandler<GetPaginatedAccountsQuery, PaginatedList<AccountListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<AccountListItem>> HandleAsync(GetPaginatedAccountsQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetPaginatedAccountsModel(
            query.PageNumber,
            query.PageSize,
            query.MinBalance,
            query.MaxBalance,
            query.StartDate,
            query.EndDate);

        return await accountsRetrievalService.GetPaginatedAccountsAsync(filter, cancellationToken);
    }
}