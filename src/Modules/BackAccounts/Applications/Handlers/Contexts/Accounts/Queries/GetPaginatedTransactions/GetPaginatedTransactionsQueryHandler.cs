using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedTransactions;

/// <summary>
/// Handles the logic for processing <see cref="GetPaginatedTransactionsQuery"/>.
/// </summary>
public sealed class GetPaginatedTransactionsQueryHandler(
    IAccountsRetrievalService transactionsRetrievalService) : IQueryHandler<GetPaginatedTransactionsQuery, PaginatedList<TransactionListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<TransactionListItem>> HandleAsync(GetPaginatedTransactionsQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetPaginatedTransactionsModel(
            query.PageNumber,
            query.PageSize,
            query.AccountId,
            query.MinAmount,
            query.MaxAmount,
            query.Type,
            query.Category,
            query.StartDate,
            query.EndDate);

        return await transactionsRetrievalService.GetPaginatedTransactionsAsync(filter, cancellationToken);
    }
}