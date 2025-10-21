namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;

/// <summary>
/// Model used to remove a category (budget item) from a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="CategoryId">Category ID.</param>
public sealed record BudgetCategoryDeletionModel(
    Guid BudgetId,
    Guid CategoryId);