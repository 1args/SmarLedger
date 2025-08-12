namespace SmartLedger.Modules.Budgets.Contracts.Requests.Budgets;

/// <summary>
/// Represents a request to create a new budget.
/// </summary>
/// <param name="Name">Name of the budget.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
public sealed record CreateBudgetRequest(
    string Name,
    DateTime StartDate,
    DateTime EndDate);