using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

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
    /// <returns><see cref="TransactionListItem"/> containing the mapped transaction data.</returns>
    public static TransactionResponse MapToResponse(this TransactionReadModel transaction) =>
        new(transaction.Id,
            transaction.AccountId,
            transaction.UserId,
            transaction.AccountName, 
            transaction.Amount, 
            transaction.Type, 
            transaction.Category, 
            transaction.Notes,
            transaction.CreatedAt, 
            transaction.LastUpdatedAt);

    /// <summary>
    /// Maps a <see cref="TransactionReadModel"/> to a <see cref="TransactionListItem"/>.
    /// </summary>
    /// <param name="transaction">Transaction read model to map.</param>
    /// <returns><see cref="TransactionListItem"/> containing the mapped transaction data.</returns>
    public static TransactionListItem MapToListItem(this TransactionReadModel transaction) =>
        new(transaction.Id,
            transaction.Amount,
            transaction.Type, 
            transaction.Category,
            transaction.Notes, 
            transaction.CreatedAt, 
            transaction.LastUpdatedAt);
}