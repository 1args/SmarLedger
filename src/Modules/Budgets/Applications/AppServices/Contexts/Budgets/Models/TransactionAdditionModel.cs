using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Models;

/// <summary>
/// Model used to add a transaction from an account.
/// </summary>
/// <param name="AccountId">Account ID.</param>
/// <param name="UserId">User ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
public sealed record TransactionAdditionModel(
    Guid AccountId,
    Guid UserId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt);