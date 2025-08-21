namespace SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read.Models;

/// <summary>
/// Represents a read model for an account.
/// </summary>
public sealed class AccountReadModel
{
    /// <summary>Account ID.</summary>
    public Guid Id { get; set; }

    /// <summary>ID of the user who owns this account.</summary>
    public Guid UserId { get; set; }

    /// <summary>Account name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Current balance.</summary>
    public decimal Balance { get; set; }

    /// <summary>Date and time when account was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date and time when account was last updated.</summary>
    public DateTime LastUpdatedAt { get; set; }
}