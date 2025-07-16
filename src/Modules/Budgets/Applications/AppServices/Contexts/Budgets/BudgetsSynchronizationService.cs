using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Domain.Enums;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <inheritdoc />
public sealed class BudgetsSynchronizationService(
    IRepository<BudgetReadModel, BudgetsReadDbContext> budgetsRepository,
    IRepository<BudgetItemReadModel, BudgetsReadDbContext> budgetItemsRepository,
    ILogger<IBudgetsSynchronizationService> logger): IBudgetsSynchronizationService
{
    /// <inheritdoc />
    public async Task SynchronizeBudgetCreationAsync(BudgetCreationSynchronizationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing budget creation with ID `{BudgetId}` and name `{Name}`...",
            request.BudgetId, request.Name);

        var budget = new BudgetReadModel
        {
            Id = request.BudgetId,
            UserId = request.UserId,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = request.CreatedAt,
            LastUpdatedAt = request.CreatedAt
        };

        await budgetsRepository.AddAsync(budget, cancellationToken);

        logger.LogInformation(
            "Budget with ID `{BudgetId}` was successfully synchronized after creation.",
            request.BudgetId);
    }

    /// <inheritdoc />
    public async Task SynchronizeCategoryAdditionAsync(CategoryAdditionSynchronizationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing category addition with ID `{CategoryId}` for budget with ID `{BudgetId}`...",
            request.CategoryId, request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);

        var category = new BudgetItemReadModel
        {
            Id = request.CategoryId,
            BudgetId = request.BudgetId,
            BudgetName = budget.Name,
            UserId = budget.UserId,
            Category = request.Category.ToString(),
            Limit = request.Limit,
            SpentAmount = 0.0m,
            Status = BudgetItemStatus.Active.ToString(),
            CreatedAt = request.CreatedAt,
            LastUpdatedAt = request.CreatedAt
        };

        await budgetItemsRepository.AddAsync(category, cancellationToken);

        logger.LogInformation(
            "Category with ID `{CategoryId}` for budget with ID `{BudgetId}` was successfully synchronized after addition.",
            request.CategoryId, request.BudgetId);
    }

    /// <inheritdoc />
    public async Task SynchronizeCategoryRemovalAsync(CategoryRemovalSynchronizationModel request, CancellationToken cancellationToken)
    {
        var category = await GetCategoryAsync(request.CategoryId, cancellationToken);

        logger.LogInformation(
            "Synchronizing category removal with ID `{CategoryId}` for budget with ID `{BudgetId}`...",
            request.CategoryId, category.BudgetId);

        await budgetItemsRepository.DeleteAsync(category, cancellationToken);

        logger.LogInformation(
            "Category with ID `{CategoryId}` for budget with ID `{BudgetId}` was successfully synchronized after removal.",
            request.CategoryId, category.BudgetId);
    }

    /// <inheritdoc />
    public async Task SynchronizeUpdateSendingAmountAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request))
        {
            return;
        }

        logger.LogInformation(
            "Synchronizing update of spending amount amount for category `{Category}` with amount `{Amount}` for user with ID `{UserId}`...",
            request.Category, request.Amount, request.UserId);

        var activeBudgets = await GetActiveBudgetsAsync(request, cancellationToken);

        if (activeBudgets.Count == 0)
        {
            logger.LogWarning(
                "No active budgets found for user with ID `{UserId}` in synchronization context.",
                request.UserId);
            return;
        }

        var categories = activeBudgets
            .SelectMany(b => b.Items)
            .ToList();

        UpdateCategories(categories, activeBudgets, request);

        await budgetItemsRepository.UpdateRangeAsync(categories.ToArray(), cancellationToken);

        logger.LogInformation(
            "Spending amount for category `{Category}` for user with ID `{UserId}` was successfully synchronized after update.",
            request.Category, request.UserId);
    }

    /// <inheritdoc />
    public async Task SynchronizeBudgetDeletionAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Synchronizing budget deletion with ID `{BudgetId}`...", 
            request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);
        await budgetsRepository.DeleteAsync(budget, cancellationToken);

        logger.LogInformation(
            "Budget with ID `{BudgetId}` was successfully synchronized after deletion.", 
            request.BudgetId);
    }

    /// <summary>
    /// Determines whether the given transaction is an expense.
    /// </summary>
    private static bool IsExpense(TransactionAdditionModel request)
        => request.Type == TransactionType.Expense;

    /// <summary>
    /// Retrieves all active budgets for a user that match the specified category in the transaction request.
    /// </summary>
    private async Task<List<BudgetReadModel>> GetActiveBudgetsAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        return await budgetsRepository
            .Where(b => b.UserId == request.UserId &&
                        b.StartDate <= request.CreatedAt &&
                        b.EndDate >= request.CreatedAt)
            .Include(b => b.Items
                .Where(i => i.Category == request.Category.ToString()))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the spending amount and status of budget categories.
    /// </summary>
    private static void UpdateCategories(List<BudgetItemReadModel> categories, List<BudgetReadModel> budgets, TransactionAdditionModel request)
    {
        foreach (var category in categories)
        {
            category.SpentAmount += request.Amount;

            var budget = budgets.Single(b => b.Id == category.BudgetId);

            category.Status = GetUpdatedStatus(budget, category, request);
            category.LastUpdatedAt = request.CreatedAt;
        }
    }

    /// <summary>
    /// Determines the updated status of a budget category.
    /// </summary>
    private static string GetUpdatedStatus(BudgetReadModel budget, BudgetItemReadModel category, TransactionAdditionModel request)
    {
        if (budget.StartDate > request.CreatedAt || budget.EndDate < request.CreatedAt)
            return BudgetItemStatus.Inactive.ToString();

        return category.SpentAmount >= category.Limit
            ? BudgetItemStatus.Exceeded.ToString()
            : BudgetItemStatus.Active.ToString();
    }

    /// <summary>
    /// Retrieves a budget by its ID or throws if not found.
    /// </summary>
    private async Task<BudgetReadModel> GetBudgetAsync(Guid budgetId, CancellationToken cancellationToken)
    {
        var budget = await budgetsRepository
            .Where(b => b.Id == budgetId)
            .SingleOrDefaultAsync(cancellationToken);

        if (budget is null)
        {
            logger.LogWarning("Budget with ID `{BudgetId}` not found in synchronization context.", budgetId);
            throw new ReadableException($"Budget with ID '{budgetId}' was not found in synchronization context.");
        }

        return budget;
    }

    /// <summary>
    /// Retrieves a category by its ID or throws if not found.
    /// </summary>
    private async Task<BudgetItemReadModel> GetCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await budgetItemsRepository
            .Where(c => c.Id == categoryId)
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category with ID `{CategoryId}` not found in synchronization context.", categoryId);
            throw new ReadableException($"Category with ID '{categoryId}' was not found in synchronization context.");
        }
        return category;
    }
}