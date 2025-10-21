using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccountsPage;

/// <summary>
/// Handles the logic for processing <see cref="GetAccountsPageQuery"/>.
/// </summary>
public sealed class GetAccountsPageQueryHandler(
    IAccountsRetrievalService accountsRetrievalService) : IQueryHandler<GetAccountsPageQuery, PaginatedList<AccountListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<AccountListItem>> HandleAsync(GetAccountsPageQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetAccountsPageModel(
            query.PageNumber,
            query.PageSize,
            query.MinBalance,
            query.MaxBalance,
            query.StartDate,
            query.EndDate);

        return await accountsRetrievalService.GetAccountsPageAsync(filter, cancellationToken);
    }
}