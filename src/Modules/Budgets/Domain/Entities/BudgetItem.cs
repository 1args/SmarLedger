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
public sealed class BudgetItem : Entity<Guid>
{
    /// <summary>Category this item represents.</summary>
    public TransactionCategory Category { get; private set; }

    /// <summary>Spending limit for this category.</summary>
    public BudgetItemLimit Limit { get; private set; }

    /// <summary>Amount already spent in this category.</summary>
    public Money SpentAmount { get; private set; }

    /// <summary>Current status of the item (e.g., Active, Exceeded).</summary>
    public BudgetItemStatus Status { get; private set; }

    /// <summary>ID of the budget this item belongs to.</summary>
    public Guid BudgetId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private BudgetItem() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetItem(TransactionCategory category, BudgetItemLimit limit)
    {
        Category = category;
        Limit = limit;
        SpentAmount = Money.Zero;
        Status = BudgetItemStatus.Active;
    }

    /// <summary>
    /// Factory method to create a new <see cref="BudgetItem"/>.
    /// </summary>
    /// <param name="category">Transaction category.</param>
    /// <param name="limit">Spending limit for the category.</param>
    /// <returns>New instance of <see cref="BudgetItem"/>.</returns>
    /// <exception cref="DomainValidationException">Thrown if category is 'Unknown' or limit is null.</exception>
    public static BudgetItem Create(TransactionCategory category, BudgetItemLimit limit)
    {
        ArgumentNullException.ThrowIfNull(limit, nameof(limit));

        if (category == TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(category), "Transaction category cannot be 'Unknown'.");
        }

        return new(category, limit);
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

        if (amount.Value < 0)
        {
            throw new DomainValidationException(nameof(amount), "Spent amount cannot be negative.");
        }

        SpentAmount = Money.Create(SpentAmount.Value + amount.Value);
        UpdateStatus(budgetPeriod);
    }

    /// <summary>
    /// Updates the spending limit of the item.
    /// </summary>
    /// <param name="newLimit">The new limit value.</param>
    /// <param name="budgetPeriod">Period of the parent budget.</param>
    public void UpdateLimit(BudgetItemLimit newLimit, BudgetPeriod budgetPeriod)
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
            Status = BudgetItemStatus.Inactive;
        }
        else
        {
            Status = SpentAmount.Value >= Limit.Value
                ? BudgetItemStatus.Exceeded
                : BudgetItemStatus.Active;
        }
    }
}