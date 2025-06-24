using SmartLedger.Common.Domain.Exceptions;
using SmartLedger.Common.Domain.Primitives;
using SmartLedger.Modules.Transactions.Domain.ValueObjects;

namespace SmartLedger.Modules.Transactions.Domain.Entities;

/// <summary>
/// Represents a category associated with a transaction.
/// </summary>
public sealed class TransactionCategory : Entity<int>
{
    /// <summary>Transaction category name.</summary>
    public TransactionCategoryName Name { get; private set; }

    /// <summary>ID of the associated transaction.</summary>
    public Guid TransactionId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private TransactionCategory() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private TransactionCategory(TransactionCategoryName name, Guid transactionId)
    {
        Name = name;
        TransactionId = transactionId;
    }

    /// <summary>
    /// Factory method to create a new instance of the <see cref="TransactionCategory"/> class.
    /// </summary>
    /// <param name="name">Name of the category.</param>
    /// <param name="transactionId">ID of the associated transaction.</param>
    /// <returns>New <see cref="TransactionCategory"/> instance.</returns>
    /// <exception cref="DomainValidationException">Thrown if the input is invalid.</exception>
    public static TransactionCategory Create(TransactionCategoryName name, Guid transactionId)
    {
        if (transactionId == Guid.Empty)
        {
            throw new DomainValidationException(nameof(transactionId), "Transaction ID cannot be empty.");
        }

        ArgumentNullException.ThrowIfNull(name);

        return new TransactionCategory(name, transactionId);
    }
}