using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

public sealed record TransactionAddedEvent(
    Guid TransactionId,
    Guid AccountId,
    decimal Amount,
    TransactionType Type,
    TransactionCategory Category,
    DateTime CreateAt,
    string Notes) : IEvent;