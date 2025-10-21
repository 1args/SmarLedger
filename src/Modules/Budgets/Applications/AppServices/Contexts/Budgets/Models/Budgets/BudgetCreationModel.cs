namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;

/// <summary>
/// Model used to create a new budget.
/// </summary>
/// <param name="Name">Name of the budget.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
/// <param name="CreatedAt">Date and time when the budget was created.</param>
public sealed record BudgetCreationModel(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt);