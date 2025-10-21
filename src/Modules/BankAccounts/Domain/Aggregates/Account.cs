using SmartLedger.Common.Domain.Enums;
using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.Entities;
using SmartLedger.Modules.BankAccounts.Domain.Exceptions;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Domain.Aggregates;

/// <summary>
/// Represents a user's account.
/// </summary>
public sealed class Account
{
    /// <summary>Account ID.</summary>
    public AccountId Id { get; private set; }

    /// <summary>Account name.</summary>
    public AccountName Name { get; private set; }

    /// <summary>Current balance.</summary>
    public Money Balance { get; private set; }

    /// <summary>ID of the user who owns this account.</summary>
    public UserId UserId { get; private set; }

    /// <summary>Date and time when account was created.</summary>
    public CreationDate CreatedAt { get; private set; }

    private readonly List<Transaction> _transactions = [];

    /// <summary>Collection of transactions associated with this account.</summary>
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Account() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Account(
        AccountId accountId, 
        AccountName name, 
        Money balance,
        UserId userId,
        CreationDate createdAt)
    {
        Id = accountId;
        Name = name;
        Balance = balance;
        UserId = userId;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Account"/> class.
    /// </summary>
    /// <param name="accountid">Account ID.</param>
    /// <param name="name">Account name.</param>
    /// <param name="userId">User ID.</param>
    /// <param name="createdAt">Date and time of creation.</param>
    /// <returns>New <see cref="Account"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if name is null or userId is empty.</exception>
    public static Account Create(Guid accountid, string name, Guid userId, DateTime createdAt)
    {
        return new Account(
            AccountId.Create(accountid),
            AccountName.Create(name),
            Money.Zero,
            UserId.Create(userId),
            CreationDate.Create(createdAt));
    }

    /// <summary>
    /// Applies a transaction to the account, updating the balance.
    /// </summary>
    /// <param name="transaction">Transaction.</param>
    public void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Type == TransactionType.Expense && Balance.Value < transaction.Amount.Value)
        {
            throw new InsufficientFundsException(nameof(transaction), "Insufficient funds in the account.");
        }

        Balance = transaction.Type == TransactionType.Income
            ? Money.Create(Balance.Value + transaction.Amount.Value)
            : Money.Create(Balance.Value - transaction.Amount.Value);

        _transactions.Add(transaction);
    }

    /// <summary>
    /// Reverses a previously applied transaction and adjusts the balance accordingly.
    /// </summary>
    /// <param name="transaction">Transaction.</param>
    public void RevertTransaction(Transaction transaction)
    {
        if (!_transactions.Contains(transaction))
        {
            throw new InvalidAccountOperationException(nameof(transaction), "Transaction not found in account.");
        }

        Balance = transaction.Type == TransactionType.Income
            ? Money.Create(Balance.Value - transaction.Amount.Value)
            : Money.Create(Balance.Value + transaction.Amount.Value);

        _transactions.Remove(transaction);
    }
}