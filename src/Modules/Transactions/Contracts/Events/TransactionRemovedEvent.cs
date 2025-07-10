using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Contracts.Events;

/// <summary>
/// Event triggered when a transaction is removed.
/// </summary> 
/// <param name="TransactionId">Transaction ID.</param>
public sealed record TransactionRemovedEvent(
    Guid TransactionId) : Event;