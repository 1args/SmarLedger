using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;

namespace SmartLedger.Modules.Budgets.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier of an budget category as a value object.
/// </summary>
public readonly struct BudgetCategoryId 
{
    /// <summary>Budget category ID.</summary>
    public Guid Value { get; }

    /// <summary>
    /// Private constructor used by the factory <see cref="Create"/> method.
    /// </summary>
    /// <param name="value">Unique identifier of the budget category.</param>
    private BudgetCategoryId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="BudgetCategoryId"/> class.
    /// </summary>
    /// <param name="value">Unique identifier of the budget category.</param>
    /// <returns>New <see cref="BudgetCategoryId"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the provided GUID is empty.</exception>
    public static BudgetCategoryId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainValidationException(nameof(value), "BudgetId cannot be an empty GUID.");
        }

        return new(value);
    }

    /// <inheritdoc/>
    public bool Equals(BudgetCategoryId other) => Value.Equals(other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BudgetCategoryId other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    public static bool operator ==(BudgetCategoryId left, BudgetCategoryId right) => left.Equals(right);

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    public static bool operator !=(BudgetCategoryId left, BudgetCategoryId right) => !left.Equals(right);
}