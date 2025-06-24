namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Model for updating the amount of an existing transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewAmount">New amount to set.</param>
public sealed record UpdateAmountModel(
    Guid TransactionId,
    decimal NewAmount);