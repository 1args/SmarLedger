namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to remove a transaction from an account.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record TransactionRemovalModel(
    Guid TransactionId);