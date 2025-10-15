using SmartLedger.Common.Domain.Enums;
using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Domain.Entities;

/// <summary>
/// Represents a financial transaction belonging to an account.
/// </summary>
public sealed class Transaction
{
    /// <summary>Transaction ID.</summary>
    public TransactionId Id { get; private set; }

    /// <summary>Account ID.</summary>
    public AccountId AccountId { get; private set; }

    /// <summary>Transaction amount.</summary>
    public Money Amount { get; private set; }

    /// <summary>Type of transaction (Income or Expense).</summary>
    public TransactionType Type { get; private set; }

    /// <summary>Transaction category.</summary>
    public FinancialCategory Category { get; private set; }

    /// <summary>Date and time when transaction was created.</summary>
    public CreationDate CreatedAt { get; private set; }

    /// <summary>Description or notes of the transaction.</summary>
    public TransactionDescription Notes { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Transaction() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Transaction(
        TransactionId transactionId,
        AccountId accountId,
        Money amount,
        TransactionType type,
        FinancialCategory category,
        CreationDate createdAt,
        TransactionDescription notes)
    {
        Id = transactionId;
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Category = category;
        CreatedAt = createdAt;
        Notes = notes;
    }

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Transaction"/> class.
    /// </summary>
    /// <param name="transactionId">Transaction ID.</param>
    /// <param name="accountId">Account ID.</param>
    /// <param name="amount">Transaction amount.</param>
    /// <param name="type">Type of the transaction.</param>
    /// <param name="category">Category.</param>
    /// <param name="createdAt">Date and time of creation.</param>
    /// <param name="notes">Transaction notes.</param>
    /// <returns>New <see cref="Transaction"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown when input data is invalid.</exception>
    public static Transaction Create(
        Guid transactionId,
        Guid accountId,
        decimal amount,
        TransactionType type,
        FinancialCategory category,
        DateTime createdAt,
        string notes)
    {
        if (amount > Money.MaxTransactionAmount)
        {
            throw new DomainValidationException(nameof(amount), $"Transaction amount cannot exceed {Money.MaxTransactionAmount:N0}.");
        }

        return new Transaction(
            TransactionId.Create(transactionId),
            AccountId.Create(accountId),
            Money.Create(amount),
            type,
            category,
            CreationDate.Create(createdAt),
            TransactionDescription.Create(notes));
    }

    /// <summary>
    /// Updates the amount of the transaction.
    /// </summary>
    /// <param name="account">Account.</param>
    /// <param name="newAmount">New amount to assign.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="newAmount"/> is null.</exception>
    public void UpdateAmount(Account account, Money newAmount)
    {
        ArgumentNullException.ThrowIfNull(account, nameof(account));
        ArgumentNullException.ThrowIfNull(newAmount, nameof(newAmount));

        if (Amount.Equals(newAmount)) return;

        account.RevertTransaction(this);
        Amount = newAmount;
        account.ApplyTransaction(this);
    }

    /// <summary>
    /// Updates the category of the transaction.
    /// </summary>
    /// <param name="newCategory">New category.</param>
    public void Categorize(FinancialCategory newCategory)
    {
        if (Category == newCategory) return;

        Category = newCategory;
    }
}