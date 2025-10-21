namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Model used to delete a transaction from an account in synchronization context.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record TransactionDeletionSynchronizationModel(
    Guid TransactionId,
    DateTime AccountUpdatedAt);