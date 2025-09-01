namespace SmartLedger.Modules.BankAccounts.Contracts.Requests.Transactions;

/// <summary>
/// Represents the request payload for retrieving a paginated list of transactions.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="MinAmount">Minimum transaction amount filter.</param>
/// <param name="MaxAmount">Maximum transaction amount filter.</param>
/// <param name="Type">Transaction type filter.</param>
/// <param name="Category">Transaction category filter.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedTransactionsRequest(
    int PageNumber,
    int PageSize,
    decimal? MinAmount,
    decimal? MaxAmount,
    string? Type,
    string? Category,
    DateTime? StartDate,
    DateTime? EndDate);