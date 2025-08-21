using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;

namespace SmartLedger.Modules.Reports.Domain.ValueObjects;

/// <summary>
/// Represents user information as a value object.
/// </summary>
public sealed class UserInfo : ValueObject
{
    /// <summary>Unique identifier for the user.</summary>
    public Guid UserId { get; }

    /// <summary>Username of the user.</summary>
    public string Username { get; }

    /// <summary>First name of the user.</summary>
    public string FirstName { get; }

    /// <summary>Last name of the user.</summary>
    public string LastName { get; }

    /// <summary>
    /// Private constructor used by factory method.
    /// </summary>
    private UserInfo(
        Guid userId,
        string username, 
        string firstName,
        string lastName)
    {
        UserId = userId;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Factory method to create a new <see cref="BudgetDetail"/>.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="username">Username.</param>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <returns>New instance of <see cref="BudgetDetail"/>.</returns>
    /// <exception cref="DomainValidationException"></exception>
    public static UserInfo Create(
        Guid userId,
        string username,
        string firstName,
        string lastName)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(userId), "User ID cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new DomainValidationException(nameof(username), "Username cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainValidationException(nameof(firstName), "First name cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainValidationException(nameof(lastName), "Last name cannot be null or empty.");
        }

        return new(userId, username, firstName, lastName);
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}