using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using System;
using System.Collections.Generic;

namespace SmartLedger.Common.Domain.ValueObjects;

/// <summary>
/// Represents the creation date of an account as a value object.
/// </summary>
public sealed class CreationDate : ValueObject
{
    /// <summary>Account creation date (in UTC).</summary>
    public DateTime Value { get; }

    /// <summary>
    /// Private constructor used by the factory <see cref="Create"/> method.
    /// </summary>
    /// <param name="value">The creation date of the account.</param>
    private CreationDate(DateTime value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="CreationDate"/> class.
    /// </summary>
    /// <param name="value">Creation date of the account (must be in UTC).</param>
    /// <returns>New <see cref="CreationDate"/> instance.</returns>
    /// <exception cref="DomainValidationException">
    /// Thrown if the date is not in UTC or if it represents a future moment.
    /// </exception>
    public static CreationDate Create(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new DomainValidationException(nameof(value), "Creation date must be in UTC.");
        }
        if (value > DateTime.UtcNow)
        {
            throw new DomainValidationException(nameof(value), "Creation date cannot be in the future.");
        }
        return new(value);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}