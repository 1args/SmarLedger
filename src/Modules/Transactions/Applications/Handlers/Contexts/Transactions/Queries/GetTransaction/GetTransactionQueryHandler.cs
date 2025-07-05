using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Abstractions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Transactions.Queries.GetTransaction;

/// <summary>
/// Handles the logic for processing <see cref="GetTransactionQuery"/>.
/// </summary>
public sealed class GetTransactionQueryHandler(
    ITransactionsRetrievalService transactionsRetrievalService) : IQueryHandler<GetTransactionQuery, TransactionResponse>
{
    /// <inheritdoc />
    public async Task<TransactionResponse> HandleAsync(GetTransactionQuery query, CancellationToken cancellationToken)
    {
        var response = await transactionsRetrievalService.GetTransactionAsync(query.TransactionId, cancellationToken);

        return response;
    }
}