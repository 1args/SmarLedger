using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategoriesPage;

/// <summary>
/// Handles the logic for processing <see cref="GetBudgetCategoriesPageQuery"/>.
/// </summary>
public sealed class GetBudgetCategoriesPageQueryHandler(
    IBudgetsRetrievalService budgetsRetrievalService) : IQueryHandler<GetBudgetCategoriesPageQuery, PaginatedList<BudgetCategoryListItem>>
{
    /// <inheritdoc />
    public async Task<PaginatedList<BudgetCategoryListItem>> HandleAsync(GetBudgetCategoriesPageQuery query, CancellationToken cancellationToken)
    {
        var filter = new GetBudgetCategoriesPageModel(
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

        return await budgetsRetrievalService.GetBudgetCategoriesPageAsync(filter, cancellationToken);
    }
}