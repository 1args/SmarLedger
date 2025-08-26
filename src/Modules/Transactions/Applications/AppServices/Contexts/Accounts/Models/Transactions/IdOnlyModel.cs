namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Model containing only the transaction ID.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record IdOnlyModel(
    Guid TransactionId);
