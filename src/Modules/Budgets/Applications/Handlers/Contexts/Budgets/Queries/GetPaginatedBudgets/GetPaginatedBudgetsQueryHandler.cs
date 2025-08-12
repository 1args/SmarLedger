using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgets;

/// <summary>
/// Handles the logic for processing <see cref="GetPaginatedBudgetsQuery"/>.
/// </summary>
public sealed class GetPaginatedBudgetsQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetPaginatedBudgetsQuery, PaginatedList<BudgetListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<BudgetListItem>> HandleAsync(GetPaginatedBudgetsQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetPaginatedBudgetsModel(
            query.PageNumber,
            query.PageSize,
            query.StartDate,
            query.EndDate);

        return await budgetsRetrievalService.GetPaginatedBudgetsAsync(filter, cancellationToken);
    }
}