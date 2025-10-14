using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier of a transaction as a value object.
/// </summary>
public readonly struct TransactionId
{
    /// <summary>Transaction ID.</summary>
    public Guid Value { get; }

    /// <summary>
    /// Private constructor used by the factory <see cref="Create"/> method.
    /// </summary>
    /// <param name="value">Unique identifier of the transaction.</param>
    private TransactionId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="TransactionId"/> class.
    /// </summary>
    /// <param name="value">Unique identifier of the transaction.</param>
    /// <returns>New <see cref="TransactionId"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the provided GUID is empty.</exception>
    public static TransactionId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainValidationException("TransactionId cannot be an empty GUID.", nameof(value));
        }

        return new(value);
    }

    /// <inheritdoc/>
    public bool Equals(TransactionId other) => Value.Equals(other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is TransactionId other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    public static bool operator ==(TransactionId left, TransactionId right) => left.Equals(right);

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    public static bool operator !=(TransactionId left, TransactionId right) => !left.Equals(right);
}