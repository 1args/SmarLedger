using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetsPage;

/// <summary>
/// Handles the logic for processing <see cref="GetBudgetsPageQuery"/>.
/// </summary>
public sealed class GetBudgetsPageQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetBudgetsPageQuery, PaginatedList<BudgetListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<BudgetListItem>> HandleAsync(GetBudgetsPageQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetBudgetsPageModel(
            query.PageNumber,
            query.PageSize,
            query.StartDate,
            query.EndDate);

        return await budgetsRetrievalService.GetBudgetsPageAsync(filter, cancellationToken);
    }
}