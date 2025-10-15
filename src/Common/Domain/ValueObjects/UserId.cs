using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLedger.Common.Domain.ValueObjects;

/// <summary>
/// Represents the unique identifier of a user as a value object.
/// </summary>
public readonly struct UserId : IEquatable<UserId>
{
    /// <summary>User ID.</summary>
    public Guid Value { get; }

    /// <summary>
    /// Private constructor used by the factory <see cref="Create"/> method.
    /// </summary>
    /// <param name="value">Unique identifier of the user.</param>
    private UserId(Guid value) => Value = value;

    /// <summary>
    /// Factory method to create a new instance of the <see cref="UserId"/> class.
    /// </summary>
    /// <param name="value">Unique identifier of the user.</param>
    /// <returns>New <see cref="UserId"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the provided GUID is empty.</exception>
    public static UserId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainValidationException("UserId cannot be an empty GUID.", nameof(value));
        }

        return new(value);
    }

    /// <inheritdoc/>
    public bool Equals(UserId other) => Value.Equals(other.Value);

    /// <inheritdoc/>
    public override bool Equals(object obj) => obj is UserId other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Equality comparison operator.
    /// </summary>
    public static bool operator ==(UserId left, UserId right) => left.Equals(right);

    /// <summary>
    /// Inequality comparison operator.
    /// </summary>
    public static bool operator !=(UserId left, UserId right) => !left.Equals(right);
}