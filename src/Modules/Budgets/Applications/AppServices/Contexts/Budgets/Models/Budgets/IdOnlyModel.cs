namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;

/// <summary>
/// Model containing only the budget ID.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
public sealed record IdOnlyModel(
    Guid BudgetId);