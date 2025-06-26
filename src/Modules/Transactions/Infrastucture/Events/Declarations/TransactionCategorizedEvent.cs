using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Transactions.Domain.Enums;

namespace SmartLedger.Modules.Transactions.Infrastructure.Events.Declarations;

public sealed record TransactionCategorizedEvent(
    Guid TransactionId,
    TransactionCategory NewCategory) : IEvent;