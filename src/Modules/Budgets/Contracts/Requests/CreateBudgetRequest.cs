namespace SmartLedger.Modules.Budgets.Contracts.Requests;

/// <summary>
/// Represents a request to create a new budget.
/// </summary>
/// <param name="UserId">ID of the user who owns the budget.</param>
/// <param name="Name">Name of the budget.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
public sealed record CreateBudgetRequest(
    Guid UserId,
    string Name,
    DateTime StartDate,
    DateTime EndDate);