using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

/// <summary>
/// Represents the description or note of a transaction as a value object.
/// </summary>
public sealed class TransactionDescription : ValueObject
{
    /// <summary>Maximum allowed length for the description.</summary>
    public const int MaxLength = 200;

    /// <summary>Description.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private TransactionDescription() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private TransactionDescription(string value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="TransactionDescription"/> class.
    /// </summary>
    /// <param name="value">Description.</param>
    /// <returns>New <see cref="TransactionDescription"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the value is invalid.</exception>
    public static TransactionDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(nameof(value), "Transaction description cannot be null or empty.");
        }
        if (value.Length > 200)
        {
            throw new DomainValidationException(nameof(value), $"Transaction description cannot exceed '{MaxLength}' characters.");
        }
        return new(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}