using SmartLedger.Modules.Transactions.Contracts.Responses;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Contracts.Mappers;

/// <summary>
/// Transaction mapper for converting read models to response models.
/// </summary>
public static class TransactionMapper
{
    /// <summary>
    /// Maps a <see cref="TransactionReadModel"/> to a <see cref="TransactionResponse"/>.
    /// </summary>
    /// <param name="transaction">Transaction read model to map.</param>
    /// <returns><see cref="TransactionResponse"/> containing the mapped transaction data.</returns>
    public static TransactionResponse MapToResponse(this TransactionReadModel transaction) =>
        new TransactionResponse(
            TransactionId: transaction.Id,
            Amount: transaction.Amount,
            Type: transaction.Type,
            Category: transaction.Category,
            Notes: transaction.Notes,
            CreatedAt: transaction.CreatedAt,
            LastUpdatedAt: transaction.LastUpdatedAt);
}