using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetPaginatedTransactions;

/// <summary>
/// Handles the logic for processing <see cref="GetPaginatedTransactionsQuery"/>.
/// </summary>
public sealed class GetPaginatedTransactionsQueryHandler(
    ITransactionsRetrievalService transactionsRetrievalService) : IQueryHandler<GetPaginatedTransactionsQuery, PaginatedList<TransactionListItem>>
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