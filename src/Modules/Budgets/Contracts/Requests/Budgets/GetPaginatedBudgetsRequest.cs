namespace SmartLedger.Modules.Budgets.Contracts.Requests.Budgets;

/// <summary>
/// Represents the request payload for retrieving a paginated list of budgets.
/// </summary>
/// <param name="PageNumber">Page number.</param>
/// <param name="PageSize">Page size.</param>
/// <param name="StartDate">Filter by start date (optional).</param>
/// <param name="EndDate">Filter by end date (optional).</param>
public sealed record GetPaginatedBudgetsRequest(
    int PageNumber,
    int PageSize,
    DateTime? StartDate,
    DateTime? EndDate);