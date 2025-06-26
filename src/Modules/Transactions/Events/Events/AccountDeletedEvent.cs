using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Events.Events;

public sealed record AccountDeletedEvent(
    Guid AccountId) : IEvent;