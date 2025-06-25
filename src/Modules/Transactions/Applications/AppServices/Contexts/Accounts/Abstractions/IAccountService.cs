using SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;
using SmartLedger.Modules.Transactions.Domain.Aggregates;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Abstractions;

/// <summary>
/// Provides functionality for managing accounts.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Creates a new account using the specified model.
    /// </summary>
    /// <param name="request">Account creation model.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CreateAsync(CreateAccountModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a transaction to an existing account.
    /// </summary>
    /// <param name="request">Model describing the transaction to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AddTransactionAsync(AddTransactionModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a transaction from an existing account.
    /// </summary>
    /// <param name="request">Model containing transaction ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RemoveTransactionAsync(RemoveTransactionModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an account by its identifier.
    /// </summary>
    /// <param name="request">Model containing account ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteAsync(IdOnlyModel request, CancellationToken cancellationToken);
}