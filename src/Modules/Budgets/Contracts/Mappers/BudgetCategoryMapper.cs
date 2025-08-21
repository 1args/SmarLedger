using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Contracts.Mappers;

/// <summary>
/// Mapper for converting budget category read models to response models.
/// </summary>
public static class BudgetCategoryMapper
{
    /// <summary>
    /// Maps a <see cref="BudgetCategoryReadModel"/> to a <see cref="BudgetCategoryResponse"/>.
    /// </summary>
    /// <param name="budgetCategory">Budget category read model to map.</param>
    /// <returns><see cref="BudgetCategoryResponse"/> containing the mapped budget category data.</returns>
    public static BudgetCategoryResponse MapToResponse(this BudgetCategoryReadModel budgetCategory) =>
        new(budgetCategory.BudgetId,
            budgetCategory.UserId,
            budgetCategory.BudgetName,
            budgetCategory.Category,
            budgetCategory.Limit,
            budgetCategory.SpentAmount,
            budgetCategory.Status,
            budgetCategory.StartDate,
            budgetCategory.EndDate,
            budgetCategory.CreatedAt,
            budgetCategory.LastUpdatedAt);

    /// <summary>
    /// Maps a <see cref="BudgetCategoryReadModel"/> to a <see cref="BudgetCategoryListItem"/>.
    /// </summary>
    /// <param name="budgetCategory">Budget category read model to map.</param>
    /// <returns><see cref="BudgetCategoryListItem"/> containing the mapped budget category data.</returns>
    public static BudgetCategoryListItem MapToListItem(this BudgetCategoryReadModel budgetCategory) =>
        new(budgetCategory.Id,
            budgetCategory.Category,
            budgetCategory.Limit,
            budgetCategory.SpentAmount,
            budgetCategory.Status,
            budgetCategory.CreatedAt,
            budgetCategory.LastUpdatedAt);
}