using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.BankAccounts.Domain.Enums;

namespace SmartLedger.Modules.BankAccounts.Applications.Handlers.Contexts.Accounts.Events.TransactionAdded;

/// <summary>
/// Event triggered when a new transaction is added.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="AccountId">Account ID.</param>
/// <param name="UserId">User ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
/// <param name="Notes">Description or notes of the transaction.</param>
public sealed record TransactionAddedEvent(
    Guid TransactionId,
    Guid AccountId,
    Guid UserId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt,
    string Notes) : Event;