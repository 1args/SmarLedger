using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetTransactionsPage;

/// <summary>
/// Handles the logic for processing <see cref="GetTransactionsPageQuery"/>.
/// </summary>
public sealed class GetTransactionsPageQueryHandler(
    IAccountsRetrievalService transactionsRetrievalService) : IQueryHandler<GetTransactionsPageQuery, PaginatedList<TransactionListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<TransactionListItem>> HandleAsync(GetTransactionsPageQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetTransactionsPageModel(
            query.PageNumber,
            query.PageSize,
            query.AccountId,
            query.MinAmount,
            query.MaxAmount,
            query.Type,
            query.Category,
            query.StartDate,
            query.EndDate);

        return await transactionsRetrievalService.GetTransactionsPageAsync(filter, cancellationToken);
    }
}