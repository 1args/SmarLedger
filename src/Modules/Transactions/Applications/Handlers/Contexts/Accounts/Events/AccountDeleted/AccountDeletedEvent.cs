using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Applications.Handlers.Contexts.Accounts.Events.AccountDeleted;

/// <summary>
/// Event triggered when an account is deleted.
/// </summary>
/// <param name="AccountId">Account ID.</param>
public sealed record AccountDeletedEvent(
    Guid AccountId) : Event;