using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;
using SmartLedger.Modules.Transactions.Contracts.Responses;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

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
    Task<TransactionReadModel> GetTransactionAsync(Guid transactionId, CancellationToken cancellationToken);
    
    Task<PaginatedList<TransactionResponse>> GetPaginatedTransactionsAsync(GetPaginatedTransactionsModel filter,
        CancellationToken cancellationToken);
}