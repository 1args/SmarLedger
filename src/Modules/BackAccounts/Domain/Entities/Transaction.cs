using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Common.Domain.ValueObjects;
using SmartLedger.Modules.BankAccounts.Domain.Aggregates;
using SmartLedger.Modules.BankAccounts.Domain.Enums;
using SmartLedger.Modules.BankAccounts.Domain.ValueObjects;

namespace SmartLedger.Modules.BankAccounts.Domain.Entities;

/// <summary>
/// Represents a financial transaction belonging to an account.
/// </summary>
public sealed class Transaction : Entity<Guid>
{
    /// <summary>Account ID.</summary>
    public Guid AccountId { get; private set; }

    /// <summary>Transaction amount.</summary>
    public Money Amount { get; private set; }

    /// <summary>Type of transaction (Income or Expense).</summary>
    public TransactionType Type { get; private set; }

    /// <summary>Transaction category.</summary>
    public TransactionCategory Category { get; private set; }

    /// <summary>Date and time when transaction was created.</summary>
    public DateTime CreatedAt { get; private set; }

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
        Guid accountId,
        Money amount,
        TransactionType type,
        TransactionCategory category,
        DateTime createdAt,
        TransactionDescription notes)
    {
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
    /// <param name="accountId">Account identifier.</param>
    /// <param name="amount">Transaction amount.</param>
    /// <param name="type">Type of the transaction.</param>
    /// <param name="category">Category.</param>
    /// <param name="createdAt">Date and time of creation.</param>
    /// <param name="notes">Transaction notes.</param>
    /// <returns>New <see cref="Transaction"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown when input data is invalid.</exception>
    public static Transaction Create(
        Guid accountId,
        Money amount,
        TransactionType type,
        TransactionCategory category,
        DateTime createdAt,
        TransactionDescription notes)
    {
        if (accountId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(accountId), "Account ID cannot be empty.");
        }
        if (category == TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(category), "Transaction category cannot be 'Unknown'.");
        }

        ArgumentNullException.ThrowIfNull(amount, nameof(amount));
        ArgumentNullException.ThrowIfNull(notes, nameof(notes));

        return new(accountId, amount, type, category, createdAt, notes);
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
    /// <exception cref="DomainValidationException">Thrown when category is unknown.</exception>
    public void Categorize(TransactionCategory newCategory)
    {
        if (newCategory == TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(newCategory), "Transaction category cannot be 'Unknown'.");
        }

        if (Category == newCategory) return;

        Category = newCategory;
    }
}