using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Contracts.Events;

/// <summary>
/// Event triggered when the amount of a transaction is updated.
/// </summary>
/// <param name="TransactionId">Transaction ID.</param>
/// <param name="NewAmount">New amount to set.</param>
/// <param name="UpdatedAt">Date and time when the category was updated.</param>
public sealed record TransactionAmountUpdatedEvent(
    Guid TransactionId,
    decimal NewAmount,
    DateTime UpdatedAt) : Event;