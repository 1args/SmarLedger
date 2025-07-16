namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Model used to synchronize the removal of a budget category.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record CategoryRemovalSynchronizationModel(
    Guid CategoryId);