using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Extensions;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Write.BudgetCategories;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Write.Budgets;
using SmartLedger.Modules.Budgets.Domain.Aggregates;
using SmartLedger.Modules.Budgets.Domain.Entities;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Write;
using SmartLedger.Modules.Transactions.Domain.Enums;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets;

/// <inheritdoc />
public sealed class BudgetsService(
    IRepository<Budget, BudgetsWriteDbContext> budgetsRepository,
    IRepository<BudgetCategory, BudgetsWriteDbContext> budgetItemsRepository,
    ITransactionManager transactionManager,
    ILogger<BudgetsService> logger) : IBudgetsService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(BudgetCreationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating budget with name {Name} for user with ID {UserId}", 
            request.Name, request.UserId);

        var name = BudgetName.Create(request.Name);
        var period = BudgetPeriod.Create(request.StartDate, request.EndDate);

        var budget = Budget.Create(request.UserId, name, period, request.CreatedAt);

        await budgetsRepository.AddAsync(budget, cancellationToken);

        logger.LogInformation(
            "Budget with ID {BudgetId} created successfully for user with ID {UserId}",
            budget.Id, request.UserId);

        return budget.Id;
    }

    /// <inheritdoc />
    public async Task<Guid> AddCategoryAsync(CategoryAdditionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Adding category {Category} with limit {Limit} to budget with ID {BudgetId}",
            request.Category, request.Limit, request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, useInclude: true, cancellationToken : cancellationToken);

        var limit = BudgetCategoryLimit.Create(request.Limit);
        var category = BudgetCategory.Create(request.Category, limit, request.CreatedAt);

        budget.AddCategory(category);

        await transactionManager.StartEffect(async ct =>
        {
            await budgetItemsRepository.AddAsync(category, ct);
            await budgetsRepository.UpdateAsync(budget, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Category {Category} with limit {Limit} added to budget with ID {BudgetId}",
            request.Category, request.Limit, request.BudgetId);

        return category.Id;
    }

    /// <inheritdoc />
    public async Task RemoveCategoryAsync(CategoryRemovalModel request, CancellationToken cancellationToken)
    {
        var category = await GetCategoryAsync(request.CategoryId, cancellationToken);

        logger.LogInformation(
            "Removing category with ID {CategoryId} from budget with ID {BudgetId}",
            request.CategoryId, category.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);

        budget.RemoveCategory(category);

        await transactionManager.StartEffect(async ct =>
        {
            await budgetItemsRepository.DeleteAsync(category, ct);
            await budgetsRepository.UpdateAsync(budget, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation(
            "Category with ID {CategoryId} removed from budget with ID {BudgetId}",
            request.CategoryId, category.BudgetId);
    }

    /// <inheritdoc />
    public async Task AddSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request)) return;

        await ProcessSpendingAsync(
            request, 
            cancellationToken,
            (item, amount, period) => item.AddExpense(amount, period));
    }

    /// <inheritdoc />
    public async Task RevertSpendingAmountAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request)) return;

        await ProcessSpendingAsync(
            request,
            cancellationToken,
            (item, amount, period) => item.RevertExpense(amount, period));
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting budget with ID {BudgetId}", request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);
        await budgetsRepository.DeleteAsync(budget, cancellationToken);

        logger.LogInformation("Budget with ID {BudgetId} deleted successfully", request.BudgetId);
    }

    /// <summary>
    /// Determines whether the given transaction is an expense.
    /// </summary>
    private static bool IsExpense(TransactionModificationModel request)
        => request.Type == TransactionType.Expense;

    /// <summary>
    /// Processes spending updates for budgets by applying the specified action to each category.
    /// </summary>
    private async Task ProcessSpendingAsync(
        TransactionModificationModel request,
        CancellationToken cancellationToken,
        Action<BudgetCategory, Money, BudgetPeriod> updateAction)
    {
        logger.LogInformation(
            "Updating spending amount for category {request.Category} with amount {request.Amount} for user with ID {request.UserId}",
            request.Category, request.Amount, request.UserId);

        var budgets = await GetActiveBudgetsAsync(request, cancellationToken);
        CheckBudgetsExistence(budgets, request);

        var amount = Money.Create(request.Amount);

        budgets
            .SelectMany(b => b.Categories
                .Select(bi => new { BudgetItem = bi, b.Period }))
            .ToList()
            .ForEach(x => updateAction(x.BudgetItem, amount, x.Period));

        await budgetsRepository.UpdateRangeAsync(budgets.ToArray(), cancellationToken);

        logger.LogInformation(
            "Spending amount updated for category {request.Category} with amount {request.Amount} for user with ID {request.UserId}",
            request.Category, request.Amount, request.UserId);
    }

    /// <summary>
    /// Retrieves all active budgets for a user that match the specified category in the transaction request.
    /// </summary>
    private async Task<List<Budget>> GetActiveBudgetsAsync(TransactionModificationModel request, CancellationToken cancellationToken)
    {
        var combinedSpecification = new BudgetByUserIdSpecification(request.UserId)
            .And(new ActiveBudgetSpecification(request.CreatedAt))
            .And(new BudgetByCategorySpecification(request.Category));

        var budget = await budgetsRepository
            .AsQueryable()
            .Where(combinedSpecification)
            .Include(b => b.Categories)
            .ToListAsync(cancellationToken);

        return budget;
    }

    /// <summary>
    ///  Checks if any active budgets exist and throws an exception if none are found.
    /// </summary>
    private void CheckBudgetsExistence(List<Budget> budgets, TransactionModificationModel request)
    {
        if (budgets.Count != 0) return;

        logger.LogWarning(
            "No active budgets found for user with ID {UserId} and category {Category}",
            request.UserId, request.Category);
        throw new NotFoundException(
            $"No active budgets found for user with ID '{request.UserId}' and category '{request.Category}'.");
    }

    /// <summary>
    /// Retrieves a budget by its ID or throws if not found.
    /// </summary>
    private async Task<Budget> GetBudgetAsync(Guid budgetId, CancellationToken cancellationToken, bool useInclude = false)
    {
        var query = budgetsRepository
            .Where(b => b.Id == budgetId);

        query = useInclude
            ? query.Include(b => b.Categories)
            : query;
       
        var budget = await query.SingleOrDefaultAsync(cancellationToken);

        if (budget is null)
        {
            logger.LogWarning("Budget with ID {AccountId} not found", budgetId);
            throw new NotFoundException($"Budget with ID '{budgetId}' was not found.");
        }

        return budget;
    }

    /// <summary>
    /// Retrieves a category by its ID or throws if not found.
    /// </summary>
    private async Task<BudgetCategory> GetCategoryAsync(Guid budgetItemId, CancellationToken cancellationToken)
    {
        var category = await budgetItemsRepository
            .Where(bi => bi.Id == budgetItemId)
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category with ID {CategoryId} not found", budgetItemId);
            throw new NotFoundException($"Category with ID '{budgetItemId}' was not found.");
        }

        return category;
    }
}