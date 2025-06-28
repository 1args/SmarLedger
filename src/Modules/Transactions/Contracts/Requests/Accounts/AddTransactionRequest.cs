using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Contracts.Requests.Accounts;

/// <summary>
/// Represents the request payload to add a transaction to an account.
/// </summary>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record AddTransactionRequest(
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    string Notes);