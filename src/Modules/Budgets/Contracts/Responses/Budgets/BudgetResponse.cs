namespace SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

/// <summary>
/// Represents a response containing the details of a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="UserId">User ID.</param>
/// <param name="Name">Budget name.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
/// <param name="CreatedAt">Date and time when budget was created.</param>
/// <param name="LastUpdatedAt">Date and time when budget was last updated.</param>
public sealed record BudgetResponse(
    Guid BudgetId,
    Guid UserId,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);