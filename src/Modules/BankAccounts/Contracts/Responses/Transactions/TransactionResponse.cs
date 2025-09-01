namespace SmartLedger.Modules.BankAccounts.Contracts.Responses.Transactions;

/// <summary>
/// Represents a response containing the details of a transaction.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="UserId">User ID.</param>
/// <param name="AccountName">Account name.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (Income or Expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
/// <param name="Amount">Date and time when transaction was created.</param>
/// <param name="LastUpdatedAt">Date and time when transaction was last updated.</param>
public record TransactionResponse(
    Guid TransactionId,
    Guid AccountId,
    Guid UserId,
    string AccountName,
    decimal Amount,
    string Type,
    string Category,
    string Notes,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);