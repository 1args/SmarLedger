using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetPaginatedBudgetCategories;

/// <summary>
/// Handles the logic for processing <see cref="GetPaginatedBudgetCategoriesQuery"/>.
/// </summary>
public sealed class GetPaginatedBudgetCategoriesQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetPaginatedBudgetCategoriesQuery, PaginatedList<BudgetCategoryListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<BudgetCategoryListItem>> HandleAsync(GetPaginatedBudgetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetPaginatedBudgetCategoriesModel(
            query.PageNumber,
            query.PageSize,
            query.BudgetId,
            query.Category,
            query.MinLimit,
            query.MaxLimit,
            query.MinSpentAmount,
            query.MaxSpentAmount,
            query.Status,
            query.StartDate,
            query.EndDate);

        return await budgetsRetrievalService.GetPaginatedBudgetCategoriesAsync(filter, cancellationToken);
    }
}