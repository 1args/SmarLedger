using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;
using SmartLedger.Modules.Budgets.Domain.Enums;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <inheritdoc />
public sealed class BudgetsSynchronizationService(
    IRepository<BudgetReadModel, BudgetsReadDbContext> budgetsRepository,
    IRepository<BudgetCategoryReadModel, BudgetsReadDbContext> budgetItemsRepository,
    ILogger<BudgetsSynchronizationService> logger): IBudgetsSynchronizationService
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

        var category = new BudgetCategoryReadModel
        {
            Id = request.CategoryId,
            BudgetId = request.BudgetId,
            BudgetName = budget.Name,
            UserId = budget.UserId,
            Category = request.Category.ToString(),
            Limit = request.Limit,
            SpentAmount = 0.0m,
            Status = BudgetCategoryStatus.Active.ToString(),
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
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
    public async Task SynchronizeAdditionSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request)) return;

        await ProcessSpendingSynchronizationAsync(
            request,
            cancellationToken,
            (category, r) => category.SpentAmount += r.Amount);
    }

    /// <inheritdoc />
    public async Task SynchronizeReversionSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request)) return;

        await ProcessSpendingSynchronizationAsync(
            request,
            cancellationToken,
            (category, r) => category.SpentAmount -= r.Amount);
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
    private static bool IsExpense(TransactionModificationModel request)
        => request.Type == TransactionType.Expense;

    /// <summary>
    /// Processes spending synchronization for categories by applying the specified action.
    /// </summary>
    private async Task ProcessSpendingSynchronizationAsync(
        TransactionModificationModel request,
        CancellationToken cancellationToken,
        Action<BudgetCategoryReadModel, TransactionModificationModel> updateAction)
    {
        logger.LogInformation(
            "Synchronizing update of spending amount amount for category `{Category}` with amount `{Amount}` and user with ID `{UserId}`...",
            request.Category, request.Amount, request.UserId);

        var categories = await GetActiveCategoriesAsync(request, cancellationToken);
        CheckCategoriesExistence(categories, request);

        categories.ForEach(c =>
        {
            updateAction(c, request);
            c.Status = GetUpdatedStatus(c, request);
            c.LastUpdatedAt = request.CreatedAt;
        });

        await budgetItemsRepository.UpdateRangeAsync(categories.ToArray(), cancellationToken);

        logger.LogInformation(
            "Spending amount for category `{Category}` and user with ID `{UserId}` was successfully synchronized after update.",
            request.Category, request.UserId);
    }

    /// <summary>
    /// Retrieves all active categories for a user that match the specified category in the transaction request.
    /// </summary>
    private async Task<List<BudgetCategoryReadModel>> GetActiveCategoriesAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        var combinedSpecification = new BudgetCategoryByUserIdSpecification(request.UserId)
            .And(new ActiveBudgetCategorySpecification(request.CreatedAt))
            .And(new BudgetCategoryByCategorySpecification(request.Category.ToString()));

        var categories = await budgetItemsRepository
            .AsQueryable()
            .Where(combinedSpecification)
            .ToListAsync(cancellationToken);

        return categories;
    }

    /// <summary>
    /// Checks if any active categories exist and throws an exception if none are found.
    /// </summary>
    private void CheckCategoriesExistence(List<BudgetCategoryReadModel> categories, TransactionModificationModel request)
    {
        if (categories.Count != 0) return;

        logger.LogWarning(
            "No active categories found for category `{Category}` and user with ID `{UserId}` in synchronization context.",
            request.Category, request.UserId);
        throw new NotFoundException(
            $"No active budgets found for user with ID '{request.UserId}' and category '{request.Category}' in synchronization context.");
    }

    /// <summary>
    /// Determines the updated status of a budget category.
    /// </summary>
    private static string GetUpdatedStatus(BudgetCategoryReadModel category, TransactionModificationModel request)
    {
        if (category.StartDate > request.CreatedAt || category.EndDate < request.CreatedAt)
            return BudgetCategoryStatus.Inactive.ToString();

        return category.SpentAmount >= category.Limit
            ? BudgetCategoryStatus.Exceeded.ToString()
            : BudgetCategoryStatus.Active.ToString();
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
    private async Task<BudgetCategoryReadModel> GetCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
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