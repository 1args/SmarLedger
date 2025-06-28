using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Contracts.Requests.Transactions;

/// <summary>
/// Represents the request payload to categorize a transaction.
/// </summary>
/// <param name="NewCategory">New category.</param>
public sealed record CategorizeTransactionRequest(
    TransactionCategory NewCategory);