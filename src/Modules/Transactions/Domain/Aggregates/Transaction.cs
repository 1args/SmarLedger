using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Modules.Transactions.Domain.Enums;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Domain.Aggregates;

/// <summary>
/// Represents a financial transaction belonging to an account.
/// </summary>
public sealed class Transaction : AggregateRoot<Guid>
{
    /// <summary>Account ID.</summary>
    public Guid AccountId { get; private set; }

    /// <summary>Transaction amount.</summary>
    public Money Amount { get; private set; }

    /// <summary>Type of transaction (Income or Expense).</summary>
    public TransactionType Type { get; private set; }

    /// <summary>Category ID associated with the transaction.</summary>
    public int CategoryId { get; private set; }

    /// <summary>Date and time when the transaction was created.</summary>
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
        int categoryId,
        DateTime createdAt,
        TransactionDescription notes)
    {
        AccountId = accountId;
        Amount = amount;
        Type = type;
        CategoryId = categoryId;
        CreatedAt = createdAt;
        Notes = notes;
    }

    /// <summary>
    /// Factory method to create a new instance of the <see cref="Transaction"/> class.
    /// </summary>
    /// <param name="accountId">Account identifier.</param>
    /// <param name="amount">Transaction amount.</param>
    /// <param name="type">Type of the transaction.</param>
    /// <param name="category">Category ID.</param>
    /// <param name="createdAt">Date and time of creation.</param>
    /// <param name="notes">Transaction notes.</param>
    /// <returns>New <see cref="Transaction"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown when input data is invalid.</exception>
    public static Transaction Create(
        Guid accountId,
        Money amount,
        TransactionType type,
        int category,
        DateTime createdAt,
        TransactionDescription notes)
    {
        if (accountId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(accountId), "Account ID cannot be empty.");
        }
        if (category == (int)TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(category), "Transaction category cannot be 'Unknown'.");
        }

        ArgumentNullException.ThrowIfNull(amount, nameof(amount));
        ArgumentNullException.ThrowIfNull(notes, nameof(notes));

        return new Transaction(accountId, amount, type, category, createdAt, notes);
    }

    /// <summary>
    /// Updates the amount of the transaction.
    /// </summary>
    /// <param name="newAmount">New amount to assign.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="newAmount"/> is null.</exception>
    public void UpdateAmount(Money newAmount)
    {
        ArgumentNullException.ThrowIfNull(newAmount, nameof(newAmount));

        if (Amount.Equals(newAmount)) return;

        Amount = newAmount;
    }

    /// <summary>
    /// Updates the category of the transaction.
    /// </summary>
    /// <param name="newCategoryId">New category ID.</param>
    /// <exception cref="DomainValidationException">Thrown when category is unknown.</exception>
    public void Categorize(int newCategoryId)
    {
        if (newCategoryId == (int)TransactionCategory.Unknown)
        {
            throw new DomainValidationException(nameof(newCategoryId), "Transaction category cannot be 'Unknown'.");
        }

        if (CategoryId == newCategoryId) return;

        CategoryId = newCategoryId;
    }
}