using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Modules.Budgets.Domain.Enums;
using SmartLedger.Modules.Budgets.Domain.ValueObjects;
using SmartLedger.Modules.Transactions.Domain.Enums;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Domain.Entities;

/// <summary>
/// Represents a single category within a budget, tracking limit and spending.
/// </summary>
public sealed class BudgetCategory : Entity<Guid>
{
    /// <summary>Category this item represents.</summary>
    public TransactionCategory Category { get; private set; }

    /// <summary>Spending limit for this category.</summary>
    public BudgetCategoryLimit Limit { get; private set; }

    /// <summary>Amount already spent in this category.</summary>
    public Money SpentAmount { get; private set; }

    /// <summary>Current status of the item (e.g., Active, Exceeded).</summary>
    public BudgetCategoryStatus Status { get; private set; }

    /// <summary>Date and time when budget item was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>ID of the budget this item belongs to.</summary>
    public Guid BudgetId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private BudgetCategory() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetCategory(TransactionCategory category, BudgetCategoryLimit limit, DateTime createdAt)
    {
        Category = category;
        Limit = limit;
        SpentAmount = Money.Zero;
        Status = BudgetCategoryStatus.Active;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Factory method to create a new <see cref="BudgetCategory"/>.
    /// </summary>
    /// <param name="category">Transaction category.</param>
    /// <param name="limit">Spending limit for the category.</param>
    /// <param name="createdAt">Date and time when budget item was created.</param>
    /// <returns>New instance of <see cref="BudgetCategory"/>.</returns>
    /// <exception cref="DomainValidationException">Thrown if category is 'Unknown' or limit is null.</exception>
    public static BudgetCategory Create(TransactionCategory category, BudgetCategoryLimit limit, DateTime createdAt)
    {
        ArgumentNullException.ThrowIfNull(limit, nameof(limit));

        if (category == TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(category), "Transaction category cannot be 'Unknown'.");
        }

        return new(category, limit, createdAt);
    }

    /// <summary>
    /// Adds an expense to the item.
    /// </summary>
    /// <param name="amount">Amount to add to spent value.</param>
    /// <param name="budgetPeriod">Period of the parent budget.</param>
    /// <exception cref="DomainValidationException">Thrown when amount is negative or null.</exception>
    public void AddExpense(Money amount, BudgetPeriod budgetPeriod)
    {
        ArgumentNullException.ThrowIfNull(amount, nameof(amount));

        SpentAmount = Money.Create(SpentAmount.Value + amount.Value);
        UpdateStatus(budgetPeriod);
    }

    /// <summary>
    /// Reverts an expense from the item, reducing the spent amount.
    /// </summary>
    /// <param name="amount">Amount to revert to spent value.</param>
    /// <param name="budgetPeriod">Period of the parent budget.</param>
    public void RevertExpense(Money amount, BudgetPeriod budgetPeriod)
    {
        ArgumentNullException.ThrowIfNull(amount, nameof(amount));

        SpentAmount = Money.Create(SpentAmount.Value - amount.Value);
        UpdateStatus(budgetPeriod);
    }

    /// <summary>
    /// Updates the spending limit of the item.
    /// </summary>
    /// <param name="newLimit">The new limit value.</param>
    /// <param name="budgetPeriod">Period of the parent budget.</param>
    public void UpdateLimit(BudgetCategoryLimit newLimit, BudgetPeriod budgetPeriod)
    {
        ArgumentNullException.ThrowIfNull(newLimit, nameof(newLimit));
        Limit = newLimit;
        UpdateStatus(budgetPeriod);
    }

    /// <summary>
    /// Updates the status of the budget item based on the spent amount and budget period.
    /// </summary>
    private void UpdateStatus(BudgetPeriod budgetPeriod)
    {
        if (!budgetPeriod.IsActive(DateTime.UtcNow))
        {
            Status = BudgetCategoryStatus.Inactive;
        }
        else
        {
            Status = SpentAmount.Value >= Limit.Value
                ? BudgetCategoryStatus.Exceeded
                : BudgetCategoryStatus.Active;
        }
    }
}