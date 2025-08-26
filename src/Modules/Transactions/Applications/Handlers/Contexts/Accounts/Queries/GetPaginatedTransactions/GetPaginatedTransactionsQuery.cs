using SmartLedger.Common.Cqrs.Queries;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetPaginatedTransactions;

/// <summary>
/// Represents a query to retrieve paginated transactions with filter.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="MinAmount">Minimum transaction amount.</param>
/// <param name="MaxAmount">Maximum transaction amount.</param>
/// <param name="Type">Transaction type.</param>
/// <param name="Category">Transaction category.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedTransactionsQuery(
    int PageNumber,
    int PageSize,
    Guid AccountId,
    decimal? MinAmount,
    decimal? MaxAmount,
    string? Type,
    string? Category,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedQuery<TransactionListItem>(PageNumber, PageSize);