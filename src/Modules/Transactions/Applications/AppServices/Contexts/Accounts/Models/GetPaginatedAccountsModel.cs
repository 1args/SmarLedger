using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Represents a request model for retrieving a paginated list of accounts.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="UserId">Account ID.</param>
/// <param name="MinBalance">Minimum transaction balance filter.</param>
/// <param name="MaxBalance">Maximum transaction balance filter.</param>
/// <param name="StartDate">Start date for filtering transactions.</param>
/// <param name="EndDate">End date for filtering transactions.</param>
public sealed record GetPaginatedAccountsModel(
    int PageNumber,
    int PageSize,
    Guid UserId,
    decimal? MinBalance,
    decimal? MaxBalance,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);