using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Contracts.Responses.Accounts;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;

/// <summary>
/// Provides functionality for receiving transactions.
/// </summary>
public interface IAccountsRetrievalService
{
    /// <summary>
    /// Retrieves an account by its identifier.
    /// </summary>
    /// <param name="accountId">Account ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a <see cref="AccountResponse"/> containing the transaction details.</returns>
    Task<AccountResponse> GetAccountAsync(Guid accountId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of accounts for a specific user.
    /// </summary>
    /// <param name="filter">Filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paginated list of accounts.</returns>
    Task<PaginatedList<AccountListItem>> GetPaginatedAccountsAsync(GetPaginatedAccountsModel filter,
        CancellationToken cancellationToken);
}