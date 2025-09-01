using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.BankAccounts.Contracts.Events;

/// <summary>
/// Event triggered when a transaction is removed.
/// </summary> 
/// <param name="UserId">User ID.</param>
/// <param name="Amount">Transaction amount.</param>
/// <param name="Type">Type of transaction (income or expense).</param>
/// <param name="Category">Transaction category.</param>
/// <param name="CreatedAt">Date and time when the transaction was created.</param>
public sealed record TransactionRemovedIntegrationEvent(
    Guid UserId,
    decimal Amount,
    TransactionType Type,
    FinancialCategory Category,
    DateTime CreatedAt) : Event;