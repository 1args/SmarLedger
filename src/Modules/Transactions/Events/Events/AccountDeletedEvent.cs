using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Events.Events;

/// <summary>
/// Event triggered when an account is deleted.
/// </summary>
/// <param name="AccountId">Account ID.</param>
public sealed record AccountDeletedEvent(
    Guid AccountId) : Event;