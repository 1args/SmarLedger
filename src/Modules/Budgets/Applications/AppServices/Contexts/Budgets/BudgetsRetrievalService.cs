using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Mappers;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <inheritdoc />
public sealed class BudgetsRetrievalService(
    IRepository<BudgetReadModel, BudgetsReadDbContext> budgetsRepository,
    IRepository<BudgetCategoryReadModel, BudgetsReadDbContext> budgetCategoriesRepository,
    ILogger<BudgetsRetrievalService> logger) : IBudgetsRetrievalService
{
    /// <inheritdoc />
    public async Task<BudgetResponse> GetBudgetAsync(Guid budgetId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget with ID {BudgetId}...", budgetId);

        var budget = await budgetsRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(b => b.Id == budgetId)
            .SingleOrDefaultAsync(cancellationToken);

        if (budget is null)
        {
            logger.LogWarning("Budget with ID `{BudgetId}` not found.", budgetId);
            throw new NotFoundException($"Budget with ID '{budgetId}' was not found.");
        }

        logger.LogInformation("Budget with ID `{BudgetId}` retrieved successfully.", budgetId);

        return budget.MapToResponse();
    }

    /// <inheritdoc />
    public async Task<PaginatedList<BudgetListItem>> GetPaginatedBudgetsAsync(GetPaginatedBudgetsModel filter, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated budgets for user with ID `{UserId}`...", filter.UserId);

        var combinedSpecification = new BudgetByUserIdSpecification(filter.UserId)
            .And(new BudgetByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var budgets = budgetsRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(combinedSpecification)
            .Select(b => b.MapToListItem());

        var paginatedBudgets = await PaginatedList<BudgetListItem>
            .CreateAsync(budgets, filter, cancellationToken);

        logger.LogInformation(
            "Successfully retrieved `{Count}` budgets (Page `{PageNumber}` of `{TotalPages}`) for user with ID `{UserId}`.",
            paginatedBudgets.Items.Count,
            paginatedBudgets.PageNumber,
            paginatedBudgets.TotalPages,
            filter.UserId);

        return paginatedBudgets;
    }

    public async Task<BudgetCategoryResponse> GetBudgetCategoryAsync(Guid budgetCategoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget category with ID {BudgetCategoryId}...", budgetCategoryId);

        var budgetCategory = await budgetCategoriesRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(bc => bc.Id == budgetCategoryId)
            .SingleOrDefaultAsync(cancellationToken);

        if (budgetCategory is null)
        {
            logger.LogWarning("Budget category with ID `{BudgetId}` not found.", budgetCategoryId);
            throw new NotFoundException($"Budget category with ID '{budgetCategoryId}' was not found.");
        }

        logger.LogInformation("Budget category with ID `{BudgetCategoryId}` retrieved successfully.", budgetCategoryId);

        return budgetCategory.MapToResponse();
    }

    public async Task<PaginatedList<BudgetCategoryListItem>> GetPaginatedBudgetCategoriesAsync(GetPaginatedBudgetCategoriesModel filter, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated budgets for budget with ID `{BudgetId}`...", filter.BudgetId);

        var combinedSpecification = new BudgetCategoryByBudgetId(filter.BudgetId)
            .And(new BudgetCategoryByCategorySpecification(filter.Category))
            .And(new BudgetCategoryByLimitRangeSpecification(filter.MinLimit, filter.MaxLimit))
            .And(new BudgetCategoryBySpentAmountSpecification(filter.MinSpentAmount, filter.MaxSpentAmount))
            .And(new BudgetCategoryByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var budgetCategories = budgetCategoriesRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(combinedSpecification)
            .Select(bc => bc.MapToListItem());

        var paginatedBudgetCategories = await PaginatedList<BudgetCategoryListItem>
            .CreateAsync(budgetCategories, filter, cancellationToken);

        logger.LogInformation(
            "Successfully retrieved `{Count}` budget categories (Page `{PageNumber}` of `{TotalPages}`) for user budget ID `{BudgetId}`.",
            paginatedBudgetCategories.Items.Count,
            paginatedBudgetCategories.PageNumber,
            paginatedBudgetCategories.TotalPages,
            filter.BudgetId);

        return paginatedBudgetCategories;
    }
}