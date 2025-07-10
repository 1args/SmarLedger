namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Model used to create a new budget.
/// </summary>
/// <param name="UserId">ID of the user who owns the budget.</param>
/// <param name="Name">Name of the budget.</param>
/// <param name="StartDate">Start date of the budget period.</param>
/// <param name="EndDate">End date of the budget period.</param>
/// <param name="CreatedAt">Date and time when the budget was created.</param>
public sealed record BudgetCreationModel(
    Guid UserId,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedAt);