namespace SmartLedger.Modules.Transactions.Contracts.Requests.Transactions;

/// <summary>
/// Represents the request payload to update the amount of a transaction.
/// </summary>
/// <param name="NewAmount">New amount.</param>
public sealed record UpdateTransactionAmountRequest(
    decimal NewAmount);