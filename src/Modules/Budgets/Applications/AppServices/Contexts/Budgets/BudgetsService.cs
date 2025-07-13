using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Contracts.Exceptions;
using SmartLedger.Common.Infrastructure.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Abstractions;
using SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;
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
    IRepository<BudgetItem, BudgetsWriteDbContext> budgetItemsRepository,
    ITransactionManager transactionManager,
    ILogger<BudgetsService> logger) : IBudgetsService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(BudgetCreationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating budget with name `{Name}` for user with ID `{Userid}`.", 
            request.UserId, request.Name);

        var name = BudgetName.Create(request.Name);
        var period = BudgetPeriod.Create(request.StartDate, request.EndDate);

        var budget = Budget.Create(request.UserId, name, period, request.CreatedAt);

        await budgetsRepository.AddAsync(budget, cancellationToken);

        logger.LogInformation("Budget with ID `{BudgetId}` created successfully for user with ID `{UserId}`.",
            budget.Id, request.UserId);

        return budget.Id;
    }

    /// <inheritdoc />
    public async Task<Guid> AddCategoryAsync(CategoryAdditionModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding category `{Category}` with limit `{Limit}` to budget with ID `{BudgetId}`.",
            request.Category, request.Limit, request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);

        var limit = BudgetItemLimit.Create(request.Limit);
        var category = BudgetItem.Create(request.Category, limit, request.CreatedAt);

        budget.AddItem(category);

        await transactionManager.StartEffect(async ct =>
        {
            await budgetItemsRepository.AddAsync(category, ct);
            await budgetsRepository.UpdateAsync(budget, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation("Category `{Category}` with limit `{Limit}` added to budget with ID `{BudgetId}`.",
            request.Category, request.Limit, request.BudgetId);

        return category.Id;
    }

    /// <inheritdoc />
    public async Task RemoveCategoryAsync(CategoryRemovalModel request, CancellationToken cancellationToken)
    {
        var category = await GetCategoryAsync(request.CategoryId, cancellationToken);

        logger.LogInformation("Removing category with ID `{CategoryId}` from budget with ID `{BudgetId}`.",
            request.CategoryId, category.BudgetId);

        var budget = await GetBudgetAsync(category.BudgetId, cancellationToken);

        budget.RemoveItem(category);

        await transactionManager.StartEffect(async ct =>
        {
            await budgetItemsRepository.DeleteAsync(category, ct);
            await budgetsRepository.UpdateAsync(budget, ct);
        }, IsolationLevel.Serializable, cancellationToken);

        logger.LogInformation("Category with ID `{CategoryId}` removed from budget with ID `{BudgetId}`.",
            request.CategoryId, category.BudgetId);
    }

    /// <inheritdoc />
    public async Task UpdateSendingAmountAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        if (!IsExpense(request))
        {
            return;
        }

        logger.LogInformation(
            "Updating spending amount for category `{Category}` with amount `{Amount}` for user with ID `{UserId}`.",
            request.Category, request.Amount, request.UserId);

        var activeBudgets = await GetActiveBudgetsAsync(request, cancellationToken);

        if (activeBudgets.Count == 0)
        {
            logger.LogWarning("No active budgets found for user with ID `{UserId}`.", request.UserId);
            return;
        }

        var amount = Money.Create(request.Amount);
        var categoriesToUpdate = activeBudgets.SelectMany(b => b.Items).ToList();

        foreach (var category in categoriesToUpdate)
        {
            var budget = activeBudgets.First(b => b.Items.Contains(category));
            category.AddExpense(amount, budget.Period);
        }

        await budgetItemsRepository.UpdateRangeAsync(categoriesToUpdate.ToArray(), cancellationToken);

        logger.LogInformation(
            "Spending amount updated for category `{Category}` with amount `{Amount}` for user with ID `{UserId}` in `{Count}` budgets.",
            request.Category, request.Amount, request.UserId, activeBudgets.Count);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting budget with ID `{BudgetId}`.", request.BudgetId);

        var budget = await GetBudgetAsync(request.BudgetId, cancellationToken);
        await budgetsRepository.DeleteAsync(budget, cancellationToken);

        logger.LogInformation("Budget with ID `{BudgetId}` deleted successfully.", request.BudgetId);
    }

    /// <summary>
    /// Determines whether the given transaction is an expense.
    /// </summary>
    private static bool IsExpense(TransactionAdditionModel request)
        => request.Type == TransactionType.Expense;

    /// <summary>
    /// Retrieves all active budgets for a user that match the specified category in the transaction request.
    /// </summary>
    private async Task<List<Budget>> GetActiveBudgetsAsync(TransactionAdditionModel request, CancellationToken cancellationToken)
    {
        return await budgetsRepository
            .Where(b => b.UserId == request.UserId && b.Period.IsActive(request.CreatedAt))
            .Include(b => b.Items.Where(item => item.Category == request.Category))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a budget by its ID or throws if not found.
    /// </summary>
    private async Task<Budget> GetBudgetAsync(Guid budgetIt, CancellationToken cancellationToken)
    {
        var budget = await budgetsRepository
            .Where(b => b.Id == budgetIt)
            .SingleOrDefaultAsync(cancellationToken);

        if (budget is null)
        {
            logger.LogWarning("Budget with ID `{AccountId}` not found.", budgetIt);
            throw new NotFoundException($"Budget with ID '{budgetIt}' was not found.");
        }

        return budget;
    }

    /// <summary>
    /// Retrieves a category by its ID or throws if not found.
    /// </summary>
    private async Task<BudgetItem> GetCategoryAsync(Guid budgetItemId, CancellationToken cancellationToken)
    {
        var category = await budgetItemsRepository
            .Where(c => c.Id == budgetItemId)
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            logger.LogWarning("Category with ID `{CategoryId}` not found.", budgetItemId);
            throw new NotFoundException($"Category with ID '{budgetItemId}' was not found.");
        }

        return category;
    }
}