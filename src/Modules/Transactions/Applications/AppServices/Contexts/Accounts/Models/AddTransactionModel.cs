using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Models;

/// <summary>
/// Model used to add a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreateAt">Date and time when the transaction was created.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record AddTransactionModel(
    Guid AccountId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreateAt,
    string Notes);