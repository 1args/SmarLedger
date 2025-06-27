namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

/// <summary>
/// Represents a read model for a transaction.
/// </summary>
public sealed class TransactionReadModel
{
    /// <summary>Transaction ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Account ID.</summary>
    public Guid AccountId { get; set; }

    /// <summary>ID of the user who owns this transaction.</summary>
    public Guid UserId { get; set; }

    /// <summary>Transaction amount.</summary>
    public decimal Amount { get; set; }

    /// <summary>Type of transaction (Income or Expense).</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Transaction category.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Description or notes of the transaction.</summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>Account name.</summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>Date and time when transaction was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date and time when transaction was last updated.</summary>
    public DateTime LastUpdatedAt { get; set; }
}