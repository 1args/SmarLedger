using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Represents a request model for retrieving a paginated list of transactions.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="MinAmount">Minimum transaction amount filter (optional).</param>
/// <param name="MaxAmount">Maximum transaction amount filter. (optional)</param>
/// <param name="Type">Transaction type filter. (optional)</param>
/// <param name="Category">Transaction category filter (optional).</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetTransactionsPageModel(
    int PageNumber,
    int PageSize,
    Guid AccountId,
    decimal? MinAmount,
    decimal? MaxAmount,
    string? Type,
    string? Category,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);