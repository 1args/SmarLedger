using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

public sealed record TransactionAmountUpdatedEvent(
    Guid TransactionId,
    decimal NewAmount) : IEvent;