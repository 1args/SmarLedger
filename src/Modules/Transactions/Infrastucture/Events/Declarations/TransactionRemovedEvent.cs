using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

public sealed record TransactionRemovedEvent(
    Guid AccountId,
    Guid TransactionId) : IEvent;