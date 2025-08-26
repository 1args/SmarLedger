using SmartLedger.Modules.BankAccounts.Domain.Enums;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Model used to add a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record TransactionAdditionModel(
    Guid AccountId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt,
    string Notes);