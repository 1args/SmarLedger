using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Transactions.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value as a value object.
/// </summary>
public sealed class Money : ValueObject
{
    /// <summary>Amount.</summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Money() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Money(decimal value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Money"/> class.
    /// </summary>
    /// <param name="value">Amount.</param>
    /// <returns>New <see cref="Money"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if value is negative.</exception>
    public static Money Create(decimal value)
    {
        if (value < 0)
        {
            throw new DomainValidationException(nameof(value), "Amount cannot be negative.");
        }
        return new Money(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}