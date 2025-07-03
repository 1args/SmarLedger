namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to remove a transaction from an account in synchronization context.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record TransactionRemovalSynchronizationModel(
    Guid AccountId,
    Guid TransactionId,
    DateTime AccountUpdatedAt);