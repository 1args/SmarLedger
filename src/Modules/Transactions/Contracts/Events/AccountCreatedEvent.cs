using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Transactions.Contracts.Events;

/// <summary>
/// Event triggered when a new account is created.
/// </summary>
/// <param name="Name">Name of the account.</param>
/// <param name="UserId">ID of the user who owns the account.</param>
/// <param name="CreatedAt">Date and time the account was created.</param>
public sealed record AccountCreatedEvent(
    Guid AccountId,
    string Name,
    Guid UserId,
    DateTime CreatedAt) : Event;