using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudget;

/// <summary>
/// Handles the logic for processing <see cref="GetBudgetQuery"/>.
/// </summary>
public sealed class GetBudgetQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetBudgetQuery, BudgetResponse>
{
    /// <inheritdoc />
    public async Task<BudgetResponse> HandleAsync(GetBudgetQuery query, CancellationToken cancellationToken)
    {
        return await budgetsRetrievalService.GetBudgetAsync(query.BudgetId, cancellationToken);
    }
}