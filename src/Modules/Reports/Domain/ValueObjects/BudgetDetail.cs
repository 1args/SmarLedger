using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Reports.Domain.ValueObjects;

/// <summary>
/// Represents the budgets details as a value object.
/// </summary>
public sealed class BudgetDetail : ValueObject
{
    /// <summary>Category of the budget.</summary>
    public string Category { get; }

    /// <summary>Total amount spent in this category.</summary>
    public Money Amount { get; }

    /// <summary>Spending limit for this category.</summary>
    public Money Limit { get; }

    /// <summary>Variance between the amount spent and the limit.</summary>
    public Money Variance { get; }

    /// <summary>Percentage of variance compared to the limit.</summary>
    public Money VariancePercentage { get; }

    /// <summary>Current status of the budget detail, e.g., "Active", "Exceeded".</summary>
    public string Status { get; set; }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetDetail(
        string category,
        Money amount,
        Money limit,
        Money variance,
        Money variancePercentage,
        string status)
    {
        Category = category;
        Amount = amount;
        Limit = limit;
        Variance = variance;
        VariancePercentage = variancePercentage;
        Status = status;
    }

    /// <summary>
    /// Factory method to create a new <see cref="BudgetDetail"/>.
    /// </summary>
    /// <param name="category">Category.</param>
    /// <param name="amount">Amount.</param>
    /// <param name="limit">Limit.</param>
    /// <param name="variance">Variance between the amount spent and the limit</param>
    /// <param name="variancePercentage">Variance percentage.</param>
    /// <param name="status">Current status</param>
    /// <returns>New instance of <see cref="BudgetDetail"/>.</returns>
    /// <exception cref="DomainValidationException"></exception>
    public static BudgetDetail Create(
        string category,
        Money amount,
        Money limit,
        Money variance,
        Money variancePercentage,
        string status)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DomainValidationException(nameof(category), "Category cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DomainValidationException(nameof(category), "Status cannot be empty.");
        }

        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(limit);
        ArgumentNullException.ThrowIfNull(variance);
        ArgumentNullException.ThrowIfNull(variancePercentage);

        return new(
            category,
            amount,
            limit, 
            variance,
            variancePercentage, 
            status);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Category;
        yield return Amount;
        yield return Limit;
        yield return Variance;
        yield return VariancePercentage;
        yield return Status;
    }
}