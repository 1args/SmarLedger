using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier of an account as a value object.
/// </summary>
public readonly struct AccountId : IEquatable<AccountId>
{
    /// <summary>Account ID.</summary>
    public Guid Value { get; }

    /// <summary>
    /// Private constructor used by the factory <see cref="Create"/> method.
    /// </summary>
    /// <param name="value">Unique identifier of the account.</param>
    private AccountId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="AccountId"/> class.
    /// </summary>
    /// <param name="value">Unique identifier of the account.</param>
    /// <returns>New <see cref="AccountId"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the provided GUID is empty.</exception>
    public static AccountId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainValidationException(nameof(value), "AccountId cannot be an empty GUID.");
        }

        return new(value);
    }

    /// <inheritdoc/>
    public bool Equals(AccountId other) => Value.Equals(other.Value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is AccountId other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    public static bool operator ==(AccountId left, AccountId right) => left.Equals(right);

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    public static bool operator !=(AccountId left, AccountId right) => !left.Equals(right);
}