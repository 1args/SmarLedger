using SmartLedger.Common.Contracts.Pagination;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Accounts;
using SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;

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
    Task<PaginatedList<AccountListItem>> GetAccountsPageAsync(GetAccountsPageModel filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a transaction by its identifier.
    /// </summary>
    /// <param name="request">Model containing account ID and transaction ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns a <see cref="TransactionResponse"/> containing the transaction details.</returns>
    Task<TransactionResponse> GetTransactionAsync(GetTransactionModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of transactions for a specific account.
    /// </summary>
    /// <param name="filter">Filter.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Paginated list of transactions.</returns>
    Task<PaginatedList<TransactionListItem>> GetTransactionsPageAsync(GetTransactionsPageModel filter,
        CancellationToken cancellationToken);
}