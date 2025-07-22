using SmartLedger.Common.Cqrs.Queries;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgets;

/// <summary>
/// Query to retrieve paginated budget categories based on various filters.
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
public sealed record GetPaginatedBudgetCategoriesQuery(
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
    DateTime? EndDate) : PaginatedQuery<BudgetCategoryListItem>(PageNumber, PageSize);