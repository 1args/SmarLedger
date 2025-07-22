using SmartLedger.Common.Contracts.Pagination;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Represents a request model for retrieving a paginated list of budget categories.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Transaction category.</param>
/// <param name="MinLimit">Filter by min limit (optional).</param>
/// <param name="MaxLimit">Filter by max limit (optional).</param>
/// <param name="MinSpentAmount">Filter by min spent amount (optional).</param>
/// <param name="MaxSpentAmount">Filter by max spent amount (optional).</param>
/// <param name="Status">Status (optional).</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetPaginatedBudgetCategoriesModel(
    int PageNumber,
    int PageSize,
    Guid BudgetId,
    string Category,
    decimal? MinLimit,
    decimal? MaxLimit,
    decimal? MinSpentAmount,
    decimal? MaxSpentAmount,
    string? Status,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedFilter(PageNumber, PageSize);