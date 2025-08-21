using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Authorization;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
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
    IHybridCache cache,
    ILogger<BudgetsRetrievalService> logger) : IBudgetsRetrievalService
{
    /// <inheritdoc />
    public async Task<BudgetResponse> GetBudgetAsync(Guid budgetId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget with ID {BudgetId}", budgetId);

        var cacheKey = $"budget:{budgetId}";
        var cacheOptions = new HybridCacheEntryOptions()
        {
            Expiration = TimeSpan.FromMinutes(5),
            LocalCacheExpiration = TimeSpan.FromSeconds(30)
        };

        var budget = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var result = await budgetsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(b => b.Id == budgetId)
                    .SingleOrDefaultAsync(ct);

                if (result is null)
                {
                    logger.LogWarning("Budget with ID {BudgetId} not found", budgetId);
                    throw new NotFoundException($"Budget with ID '{budgetId}' was not found.");
                }

                return result.MapToResponse();
            }, cancellationToken: cancellationToken);

        logger.LogInformation("Budget with ID {BudgetId} retrieved successfully", budgetId);
        return budget;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<BudgetListItem>> GetPaginatedBudgetsAsync(
        GetPaginatedBudgetsModel filter,
        CancellationToken cancellationToken)
    {
        var userId = authorizationData.Value.UserId;

        logger.LogInformation("Retrieving paginated budgets for user with ID {UserId}", userId);

        var cacheKey = $"budgets:user:{userId}:page:{filter.PageNumber}:start:{filter.StartDate:yyyy-MM-dd}" +
                       $":end:{filter.EndDate:yyyy-MM-dd}";

        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(1),
            LocalCacheExpiration = TimeSpan.FromSeconds(15)
        };

        var paginatedBudgets = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var combinedSpecification = new BudgetByUserIdSpecification(userId)
                    .And(new BudgetByDateRangeSpecification(filter.StartDate, filter.EndDate));

                var budgets = budgetsRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(combinedSpecification)
                    .Select(b => b.MapToListItem());

                return await PaginatedList<BudgetListItem>.CreateAsync(budgets, filter, ct);
            }, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} budgets (Page {PageNumber} of {TotalPages}) for user with ID {UserId}",
            paginatedBudgets.Items.Count,
            paginatedBudgets.PageNumber,
            paginatedBudgets.TotalPages,
            userId);

        return paginatedBudgets;
    }

    /// <inheritdoc />
    public async Task<BudgetCategoryResponse> GetBudgetCategoryAsync(Guid budgetCategoryId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving budget category with ID {BudgetCategoryId}", budgetCategoryId);

        var cacheKey = $"budgetcategory:{budgetCategoryId}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(5),
            LocalCacheExpiration = TimeSpan.FromSeconds(30)
        };

        var budgetCategory = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
                var result = await budgetCategoriesRepository
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(bc => bc.Id == budgetCategoryId)
                    .SingleOrDefaultAsync(ct);

                if (result is null)
                {
                    logger.LogWarning("Budget category with ID {BudgetCategoryId} not found", budgetCategoryId);
                    throw new NotFoundException($"Budget category with ID '{budgetCategoryId}' was not found.");
                }

                return result.MapToResponse();
            }, cancellationToken: cancellationToken);

        logger.LogInformation("Budget category with ID {BudgetCategoryId} retrieved successfully", budgetCategoryId);
        return budgetCategory;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<BudgetCategoryListItem>> GetPaginatedBudgetCategoriesAsync(
        GetPaginatedBudgetCategoriesModel filter, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving paginated budget categories for budget with ID {BudgetId}", filter.BudgetId);

        var cacheKey = $"budgetcategories:budget:{filter.BudgetId}:page:{filter.PageNumber}:category:{filter.Category}" +
                       $":minlimit:{filter.MinLimit}:maxlimit:{filter.MaxLimit}:minspent:{filter.MinSpentAmount}" +
                       $":maxspent:{filter.MaxSpentAmount}:start:{filter.StartDate:yyyy-MM-dd}:end:{filter.EndDate:yyyy-MM-dd}";
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromSeconds(30),
            LocalCacheExpiration = TimeSpan.FromSeconds(10)
        };

        var paginatedBudgetCategories = await cache.GetOrCreateAsync(
            key: cacheKey,
            options: cacheOptions,
            factory: async ct =>
            {
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

                return await PaginatedList<BudgetCategoryListItem>.CreateAsync(budgetCategories, filter, ct);
            }, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Successfully retrieved {Count} budget categories (Page {PageNumber} of {TotalPages}) for budget ID {BudgetId}",
            paginatedBudgetCategories.Items.Count,
            paginatedBudgetCategories.PageNumber,
            paginatedBudgetCategories.TotalPages,
            filter.BudgetId);

        return paginatedBudgetCategories;
    }
}