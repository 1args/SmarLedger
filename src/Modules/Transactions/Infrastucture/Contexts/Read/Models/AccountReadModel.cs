namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

public sealed class AccountReadModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}