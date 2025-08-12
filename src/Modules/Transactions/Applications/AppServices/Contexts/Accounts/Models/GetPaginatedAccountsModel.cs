using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Represents a request model for retrieving a paginated list of accounts.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="MinBalance">Minimum transaction balance filter (optional).</param>
/// <param name="MaxBalance">Maximum transaction balance filter (optional).</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetPaginatedAccountsModel(
    int PageNumber,
    int PageSize,
    decimal? MinBalance,
    decimal? MaxBalance,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);