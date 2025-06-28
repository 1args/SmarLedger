namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Model for updating the amount of an existing transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewAmount">New amount to set.</param>
/// <param name="UpdatedAt">Date and time when the amount was updated.</param>
public sealed record UpdateAmountModel(
    Guid TransactionId,
    decimal NewAmount,
    DateTime UpdatedAt);