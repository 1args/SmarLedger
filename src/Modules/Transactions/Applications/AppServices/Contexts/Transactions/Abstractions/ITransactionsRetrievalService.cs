using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;

/// <summary>
/// Provides functionality for receiving transactions.
/// </summary>
public interface ITransactionsRetrievalService
{
    /// <summary>
    /// Retrieves a transaction by its identifier.
    /// </summary>
    /// <param name="transactionId">Transaction ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a <see cref="TransactionResponse"/> containing the transaction details.</returns>
    Task<TransactionResponse> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of transactions for a specific account.
    /// </summary>
    /// <param name="filter">Filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paginated list of transactions.</returns>
    Task<PaginatedList<TransactionListItem>> GetPaginatedTransactionsAsync(GetPaginatedTransactionsModel filter,
        CancellationToken cancellationToken);
}