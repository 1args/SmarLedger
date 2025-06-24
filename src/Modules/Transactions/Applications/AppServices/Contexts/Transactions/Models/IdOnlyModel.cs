namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Model containing only the transaction ID.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
public sealed record IdOnlyModel(
    Guid TransactionId);
