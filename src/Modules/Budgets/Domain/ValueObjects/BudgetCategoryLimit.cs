using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Budgets.Domain.ValueObjects;

/// <summary>
/// Represents the monetary limit set for a budget category as a value object.
/// </summary>
public sealed class BudgetCategoryLimit : ValueObject
{
    /// <summary>Maximum allowed value for a budget limit.</summary>
    public const int MaxValue = 1_000_000;

    /// <summary>Limit.</summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private BudgetCategoryLimit() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetCategoryLimit(decimal value) => Value = value;

    /// <summary>
    /// Creates a new <see cref="BudgetCategoryLimit"/> instance.
    /// </summary>
    /// <param name="value">The limit value.</param>
    /// <returns>New <see cref="BudgetCategoryLimit"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the value is negative or exceeds maximum limit.</exception>
    public static BudgetCategoryLimit Create(decimal value)
    {
        return value switch
        {
            < 0 => throw new DomainValidationException(nameof(value), "Budget limit cannot be negative."),
            > MaxValue => throw new DomainValidationException(nameof(value), $"Budget limit cannot exceed '{MaxValue}'."),
            _ => new BudgetCategoryLimit(value)
        };
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}