namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Model used to delete a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record TransactionDeletionModel(
    Guid AccountId,
    Guid TransactionId);