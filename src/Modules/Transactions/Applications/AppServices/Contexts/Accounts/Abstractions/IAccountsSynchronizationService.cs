using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts;
using SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;
using IdOnlyModel = SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Accounts.IdOnlyModel;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Abstractions;

/// <summary>
/// Provides functionality for synchronizing changes in accounts between write and read models.
/// </summary>
public interface IAccountsSynchronizationService
{
    /// <summary>
    /// Synchronizes the creation of a new account.
    /// </summary>
    /// <param name="request">Account creation model.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeAccountCreationAsync(AccountCreationSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the addition of a transaction to an account.
    /// </summary>
    /// <param name="request">Model describing the transaction to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeTransactionAdditionAsync(TransactionAdditionSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the removal of a transaction from an account.
    /// </summary>
    /// <param name="request">Model containing transaction ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeTransactionRemovalAsync(TransactionRemovalSynchronizationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Synchronizes the deletion of an account.
    /// </summary>
    /// <param name="request">Model containing account ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SynchronizeAccountDeletionAsync(IdOnlyModel request, CancellationToken cancellationToken);
}