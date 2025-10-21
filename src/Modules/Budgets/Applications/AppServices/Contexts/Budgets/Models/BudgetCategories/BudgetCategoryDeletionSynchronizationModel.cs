namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;

/// <summary>
/// Model used to synchronize the deletion of a budget category.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record BudgetCategoryDeletionSynchronizationModel(
    Guid CategoryId);