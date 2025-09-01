using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Models.Transactions;

/// <summary>
/// Model used to add a transaction from an account in synchronization context.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record TransactionAdditionSynchronizationModel(
    Guid TransactionId,
    Guid AccountId,
    decimal Amount,
    TransactionType Type,
    FinancialCategory Category,
    DateTime CreatedAt,
    string Notes);