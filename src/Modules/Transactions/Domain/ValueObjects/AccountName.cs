using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Transactions.Domain.ValueObjects;

/// <summary>
/// Represents the name of an account as a value object.
/// </summary>
public sealed class AccountName : ValueObject
{
    /// <summary>Maximum allowed length for the account name.</summary>
    public const int MaxLength = 100;

    /// <summary>Name.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private AccountName() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private AccountName(string value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="AccountName"/> class.
    /// </summary>
    /// <param name="value">Name.</param>
    /// <returns>New <see cref="AccountName"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if value is null, empty or exceeds length.</exception>
    public static AccountName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(nameof(value), "Account name cannot be null or empty.");
        }
        if (value.Length > MaxLength)
        {
            throw new DomainValidationException(nameof(value), $"Account name cannot exceed '{MaxLength}' characters.");
        }
        return new AccountName(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}