using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Represents a request model for retrieving a paginated list of budgets.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetPaginatedBudgetsModel(
    int PageNumber,
    int PageSize,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);