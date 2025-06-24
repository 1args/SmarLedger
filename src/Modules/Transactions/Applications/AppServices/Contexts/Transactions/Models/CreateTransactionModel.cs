using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Models;

/// <summary>
/// Model used for creating a new transaction.
/// </summary>
/// <param name="AccountId">Account's unique identifier the transaction belongs to.</param>
/// <param name="Amount">Monetary amount of the transaction.</param>
/// <param name="Type">Type of the transaction (Income or Expense).</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
/// <param name="CategoryId">Category ID associated with the transaction.</param>
/// <param name="Notes">Description or notes related to the transaction.</param>
public sealed record CreateTransactionModel(
    Guid AccountId, 
    decimal Amount,
    TransactionType Type,
    DateTime CreatedAt,
    int CategoryId,
    string Notes);