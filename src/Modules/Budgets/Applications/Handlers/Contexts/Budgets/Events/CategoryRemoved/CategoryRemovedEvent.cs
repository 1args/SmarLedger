using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Events.CategoryRemoved;

/// <summary>
/// Event triggered when a category is removed from a budget.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record CategoryRemovedEvent(
    Guid CategoryId) : Event;