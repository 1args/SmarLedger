using SmartLedger.Common.Cqrs.Queries;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgets;

/// <summary>
/// Represents a query to retrieve paginated budgets with filter.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetPaginatedBudgetsQuery(
    int PageNumber,
    int PageSize,
    DateTime? StartDate,
    DateTime? EndDate) : PaginatedQuery<BudgetListItem>(PageNumber, PageSize);