using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;

/// <summary>
/// Provides functionality for receiving budgets.
/// </summary>
public interface IBudgetsRetrievalService
{
    /// <summary>
    /// Retrieves a budget by its identifier.
    /// </summary>
    /// <param name="budgetId">Budget ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a <see cref="BudgetResponse"/> containing the budget details.</returns>
    Task<BudgetResponse> GetBudgetAsync(Guid budgetId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of budgets for a specific user.
    /// </summary>
    /// <param name="filter">Filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paginated list of budgets.</returns>
    Task<PaginatedList<BudgetListItem>> GetBudgetsPageAsync(GetBudgetsPageModel filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a budget category by its identifier.
    /// </summary>
    /// <param name="budgetCategoryId">Budget category ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a <see cref="BudgetCategoryResponse"/> containing the budget category details.</returns>
    Task<BudgetCategoryResponse> GetBudgetCategoryAsync(Guid budgetCategoryId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of budget categories for a specific user.
    /// </summary>
    /// <param name="filter">Filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paginated list of budget categories.</returns>
    Task<PaginatedList<BudgetCategoryListItem>> GetBudgetCategoriesPageAsync(
        GetBudgetCategoriesPageModel filter, CancellationToken cancellationToken);
}