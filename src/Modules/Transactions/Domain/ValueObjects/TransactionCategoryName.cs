using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Transactions.Domain.ValueObjects;

/// <summary>
/// Represents the name of a transaction category as a value object.
/// </summary>
public sealed class TransactionCategoryName : ValueObject
{
    /// <summary>Maximum allowed length for the category name.</summary>
    public const int MaxLength = 25;

    /// <summary>Category name.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private TransactionCategoryName() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private TransactionCategoryName(string value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Money"/> class.
    /// </summary>
    /// <param name="value">Category name.</param>
    /// <returns>New <see cref="TransactionCategoryName"/> instance.</returns>
    /// <exception cref="DomainValidationException">
    /// Thrown if the value is null, empty, or exceeds the maximum allowed length.
    /// </exception>
    public static TransactionCategoryName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(nameof(value), "Transaction category name cannot be empty.");
        }
        if (value.Length > MaxLength)
        {
            throw new DomainValidationException(nameof(value), $"Transaction category name cannot exceed '{MaxLength}' characters.");
        }

        return new TransactionCategoryName(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}