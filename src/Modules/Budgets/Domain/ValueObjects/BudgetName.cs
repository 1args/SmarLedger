using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Budgets.Domain.ValueObjects;

/// <summary>
/// Represents the name of a budget as a value object.
/// </summary>
public sealed class BudgetName : ValueObject
{
    /// <summary>Maximum allowed length of the name.</summary>
    public const int MaxLength = 100;

    /// <summary>Name.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private BudgetName() { }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private BudgetName(string value) => Value = value;

    /// <summary>
    /// Creates a new <see cref="BudgetName"/> instance.
    /// </summary>
    /// <param name="value">Name to assign.</param>
    /// <returns>New <see cref="BudgetName"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the name is null, empty, or too long.</exception>
    public static BudgetName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(nameof(value), "Budget name cannot be null or empty.");
        }
        if (value.Length > MaxLength)
        {
            throw new DomainValidationException(nameof(value), $"Budget name cannot exceed '{MaxLength}' characters.");
        }
        return new(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}