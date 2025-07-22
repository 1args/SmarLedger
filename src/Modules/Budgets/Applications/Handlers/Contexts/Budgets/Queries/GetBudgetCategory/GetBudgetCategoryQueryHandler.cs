using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategory;

/// <summary>
/// Handles the logic for processing <see cref="GetBudgetCategoryQuery"/>.
/// </summary>
public sealed class GetBudgetCategoryQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetBudgetCategoryQuery, BudgetCategoryResponse>
{
    /// <inheritdoc />
    public async Task<BudgetCategoryResponse> HandleAsync(GetBudgetCategoryQuery query, CancellationToken cancellationToken)
    {
        return await budgetsRetrievalService.GetBudgetCategoryAsync(query.BudgetCategoryId, cancellationToken);
    }
}