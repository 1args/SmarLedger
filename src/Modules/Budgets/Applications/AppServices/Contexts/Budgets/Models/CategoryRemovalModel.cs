namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Model used to remove a category (budget item) from a budget.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record CategoryRemovalModel(
    Guid CategoryId);