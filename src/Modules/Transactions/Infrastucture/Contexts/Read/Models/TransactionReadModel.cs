namespace SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

public sealed class TransactionReadModel
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}