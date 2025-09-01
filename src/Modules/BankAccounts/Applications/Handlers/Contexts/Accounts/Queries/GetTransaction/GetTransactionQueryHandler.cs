using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetTransaction;

/// <summary>
/// Handles the logic for processing <see cref="GetTransactionQuery"/>.
/// </summary>
public sealed class GetTransactionQueryHandler(
    IAccountsRetrievalService transactionsRetrievalService) : IQueryHandler<GetTransactionQuery, TransactionResponse>
{
    /// <inheritdoc />
    public async Task<TransactionResponse> HandleAsync(GetTransactionQuery query, CancellationToken cancellationToken)
    {
        var request = new GetTransactionModel(query.AccountId, query.TransactionId);
        var response = await transactionsRetrievalService.GetTransactionAsync(request, cancellationToken);

        return response;
    }
}