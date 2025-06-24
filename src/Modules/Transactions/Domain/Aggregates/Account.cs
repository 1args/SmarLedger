using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Modules.Transactions.Domain.Enums;
using SmartLedger.Modules.Transactions.Domain.Exceptions;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Domain.Aggregates;

/// <summary>
/// Represents a user account.
/// </summary>
public sealed class Account : AggregateRoot<Guid>
{
    /// <summary>Account name.</summary>
    public AccountName Name { get; private set; }

    /// <summary>Current balance.</summary>
    public Money Balance { get; private set; }

    /// <summary>ID of the user who owns this account.</summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Account() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Account(AccountName name, Money balance, Guid userId)
    {
        Name = name;
        Balance = balance;
        UserId = userId;
    }

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Account"/> class.
    /// </summary>
    /// <param name="name">Account name.</param>
    /// <param name="userId">User ID.</param>
    /// <returns>New <see cref="Account"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if name is null or userId is empty.</exception>
    public static Account Create(AccountName name, Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(userId), "User ID cannot be empty.");
        }

        ArgumentNullException.ThrowIfNull(name, nameof(name));

        var balance = Money.Create(0.0m);
        return new Account(name, balance, userId);
    }

    /// <summary>
    /// Applies a transaction to the account, updating the balance.
    /// </summary>
    /// <param name="amount">Amount of the transaction.</param>
    /// <param name="transactionType">Type of the transaction (Income or Expense).</param>
    /// <exception cref="AccountNotAllowedOperation">Thrown if the expense exceeds available balance.</exception>
    public void AddTransaction(Money amount, TransactionType transactionType)
    {
        if (transactionType == TransactionType.Expense && Balance.Value < amount.Value)
        {
            throw new AccountNotAllowedOperation("There are not enough funds in the account.");
        }

        Balance = Money.Create
        (Balance.Value + (transactionType == TransactionType.Income
            ? amount.Value
            : -amount.Value));
    }

    /// <summary>
    /// Reverses a previously applied transaction and adjusts the balance accordingly.
    /// </summary>
    /// <param name="amount">Amount of the transaction.</param>
    /// <param name="transactionType">Type of the transaction (Income or Expense).</param>
    public void RemoveTransaction(Money amount, TransactionType transactionType)
    {
        Balance = Money.Create(
            Balance.Value + (transactionType == TransactionType.Income
                ? -amount.Value
                : amount.Value));
    }
}