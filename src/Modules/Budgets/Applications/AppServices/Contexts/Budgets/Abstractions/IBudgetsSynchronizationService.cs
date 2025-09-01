using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;

/// <summary>
/// Provides functionality for synchronizing changes in budgets between write and read models.
/// </summary>
public interface IBudgetsSynchronizationService
{
    /// <summary>
    /// Synchronizes the creation of a new budget.
    /// </summary>
    /// <param name="request">Model containing budget creation data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeBudgetCreationAsync(BudgetCreationSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the addition of a category to a budget.
    /// </summary>
    /// <param name="request">Model describing the category to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeCategoryAdditionAsync(CategoryAdditionSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the removal of a category from a budget.
    /// </summary>
    /// <param name="request">Model containing the category ID to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeCategoryRemovalAsync(CategoryRemovalSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronises the addition the spending amount by adding a transaction in the budget context.
    /// </summary>
    /// <param name="request">Model containing transaction details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeAdditionSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the reversion of a spending amount by reverting a transaction in the budget context.
    /// </summary>
    /// <param name="request">Model containing transaction details to revert.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeReversionSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the deletion of a budget.
    /// </summary>
    /// <param name="request">Model containing budget ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeBudgetDeletionAsync(IdOnlyModel request, CancellationToken cancellationToken);
}