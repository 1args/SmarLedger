using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Queries.GetAccount;

/// <summary>
/// Handles the logic for processing <see cref="GetAccountQuery"/>.
/// </summary>
public sealed class GetAccountQueryHandler(
    IAccountsRetrievalService accountsRetrievalService) : IQueryHandler<GetAccountQuery, AccountResponse>
{
    /// <inheritdoc />
    public async Task<AccountResponse> HandleAsync(GetAccountQuery query, CancellationToken cancellationToken)
    {
        return await accountsRetrievalService.GetAccountAsync(query.UserId, cancellationToken);
    }
}