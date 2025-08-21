using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Contracts.Mappers;

/// <summary>
/// Mapper for converting budget read models to response models.
/// </summary>
public static class BudgetMapper
{
    /// <summary>
    /// Maps a <see cref="BudgetReadModel"/> to a <see cref="BudgetResponse"/>.
    /// </summary>
    /// <param name="budget">Budget read model to map.</param>
    /// <returns><see cref="BudgetResponse"/> containing the mapped budget data.</returns>
    public static BudgetResponse MapToResponse(this BudgetReadModel budget) =>
        new(budget.Id,
            budget.UserId,
            budget.Name,
            budget.StartDate,
            budget.EndDate,
            budget.CreatedAt,
            budget.LastUpdatedAt);

    /// <summary>
    /// Maps a <see cref="BudgetReadModel"/> to a <see cref="BudgetListItem"/>.
    /// </summary>
    /// <param name="budget">Budget read model to map.</param>
    /// <returns><see cref="BudgetListItem"/> containing the mapped budget data.</returns>
    public static BudgetListItem MapToListItem(this BudgetReadModel budget) =>
        new(budget.Id,
            budget.Name,
            budget.StartDate,
            budget.EndDate,
            budget.CreatedAt,
            budget.LastUpdatedAt);
}