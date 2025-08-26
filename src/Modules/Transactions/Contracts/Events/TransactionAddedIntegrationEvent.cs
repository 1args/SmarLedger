using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.BankAccounts.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Contracts.Events;

/// <summary>
/// Event triggered when a new transaction is added.
/// </summary>
/// <param name="UserId">User ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
public sealed record TransactionAddedIntegrationEvent(
    Guid UserId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreatedAt) : Event;