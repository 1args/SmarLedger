using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;

/// <summary>
/// Provides functionality for managing budgets.
/// </summary>
public interface IBudgetsService
{
    /// <summary>
    /// Creates a new budget for a user.
    /// </summary>
    /// <param name="request">Model containing budget creation data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task<(Guid BudgetId, Guid UserId)> CreateBudgetAsync(BudgetCreationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new category (budget item) to an existing budget.
    /// </summary>
    /// <param name="request">Model describing the category to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task<Guid> CreateBudgetCategoryAsync(BudgetCategoryCreationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an existing category (budget item) from a budget.
    /// </summary>
    /// <param name="request">Model containing the category ID to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteBudgetCategoryAsync(BudgetCategoryDeletionModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the spending amount by adding a transaction in the budget context.
    /// </summary>
    /// <param name="request">Model containing transaction details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns></returns>
    Task AddSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Reverts the spending amount by removing a transaction in the budget context.
    /// </summary>
    /// <param name="request">Model containing transaction details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RevertSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a budget by its identifier.
    /// </summary>
    /// <param name="request">Model containing budget ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteBudgetAsync(IdOnlyModel request, CancellationToken cancellationToken);
}