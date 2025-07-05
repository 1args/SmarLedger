namespace SmartLedger.Modules.Transactions.Contracts.Responses.Transactions;

/// <summary>
/// Represents a response containing the details of a transaction in list.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (Income or Expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
/// <param name="Amount">Date and time when transaction was created.</param>
/// <param name="LastUpdatedAt">Date and time when transaction was last updated.</param>
public record TransactionListItem(
    Guid TransactionId,
    decimal Amount,
    string Type,
    string Category,
    string Notes,
    DateTime CreatedAt,
    DateTime LastUpdatedAt);