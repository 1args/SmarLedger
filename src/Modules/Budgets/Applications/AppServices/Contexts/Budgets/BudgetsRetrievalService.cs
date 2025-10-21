using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models.Budgets;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;
using SmartLedger.Modules.Budgets.Contracts.Mappers;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <inheritdoc />
public sealed class BudgetsRetrievalService(
    IRepository<BudgetReadModel, BudgetsReadDbContext> budgetsRepository,
    IRepository<BudgetCategoryReadModel, BudgetsReadDbContext> budgetCategoriesRepository,
    Lazy<IAuthorizationData> authorizationData,
    ILogger<BudgetsRetrievalService> logger) : IBudgetsRetrievalService
{
    /// <inheritdoc />
    public async Task<BudgetResponse> GetBudgetAsync(
        Guid budgetId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget with ID {BudgetId}", budgetId);

        var budget = await budgetsRepository
            .Where(b => b.Id == budgetId)
            .Select(b => b.MapToResponse())
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Budget with ID '{budgetId}' was not found.");

        logger.LogInformation("Budget with ID {BudgetId} retrieved successfully", budgetId);

        return budget;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<BudgetListItem>> GetBudgetsPageAsync(
        GetBudgetsPageModel filter,
        CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving paginated budgets for user with ID {UserId}", userId);

        var combinedSpecification = new BudgetByUserIdSpecification(userId)
            .And(new BudgetByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var budgets = budgetsRepository
            .Where(combinedSpecification.Criteria)
            .Select(b => b.MapToListItem());

        var budgetsPage = await PaginatedList<BudgetListItem>.CreateAsync(
            budgets,
            filter, 
            cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} budgets (Page {PageNumber} of {TotalPages}) for user with ID {UserId}",
            budgetsPage.Items.Count,
            budgetsPage.PageNumber,
            budgetsPage.TotalPages,
            userId);

        return budgetsPage;
    }

    /// <inheritdoc />
    public async Task<BudgetCategoryResponse> GetBudgetCategoryAsync(
        Guid budgetCategoryId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget category with ID {BudgetCategoryId}", budgetCategoryId);

        var budgetCategory = await budgetCategoriesRepository
            .Where(bc => bc.Id == budgetCategoryId)
            .Select(bc => bc.MapToResponse())
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Budget category with ID '{budgetCategoryId}' was not found.");

        logger.LogInformation("Budget category with ID {BudgetCategoryId} retrieved successfully", budgetCategoryId);

        return budgetCategory;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<BudgetCategoryListItem>> GetBudgetCategoriesPageAsync(
        GetBudgetCategoriesPageModel filter, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated budget categories for budget with ID {BudgetId}", filter.BudgetId);

        var combinedSpecification = new BudgetCategoryByBudgetId(filter.BudgetId)
            .And(new BudgetCategoryByCategorySpecification(filter.Category))
            .And(new BudgetCategoryByLimitRangeSpecification(filter.MinLimit, filter.MaxLimit))
            .And(new BudgetCategoryBySpentAmountSpecification(filter.MinSpentAmount, filter.MaxSpentAmount))
            .And(new BudgetCategoryByDateRangeSpecification(filter.StartDate, filter.EndDate));

        var budgetCategories = budgetCategoriesRepository
            .Where(combinedSpecification.Criteria)
            .Select(bc => bc.MapToListItem());

        var budgetCategoriesPage = await PaginatedList<BudgetCategoryListItem>.CreateAsync(
            budgetCategories, 
            filter,
            cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} budget categories (Page {PageNumber} of {TotalPages}) for budget ID {BudgetId}",
            budgetCategoriesPage.Items.Count,
            budgetCategoriesPage.PageNumber,
            budgetCategoriesPage.TotalPages,
            filter.BudgetId);

        return budgetCategoriesPage;
    }
}