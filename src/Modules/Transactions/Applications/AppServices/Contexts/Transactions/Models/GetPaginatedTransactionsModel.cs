using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Represents a request model for retrieving a paginated list of transactions.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="MinAmount">Minimum transaction amount filter.</param>
/// <param name="MaxAmount">Maximum transaction amount filter.</param>
/// <param name="Type">Transaction type filter.</param>
/// <param name="Category">Transaction category filter.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedTransactionsModel(
    int PageNumber,
    int PageSize,
    Guid AccountId,
    decimal? MinAmount,
    decimal? MaxAmount,
    string? Type,
    string? Category,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);