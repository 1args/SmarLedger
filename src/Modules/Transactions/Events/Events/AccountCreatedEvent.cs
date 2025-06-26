using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Events.Events;

public sealed record AccountCreatedEvent(
    Guid AccountId,
    string Name,
    Guid UserId,
    DateTime CreatedAt) : IEvent;