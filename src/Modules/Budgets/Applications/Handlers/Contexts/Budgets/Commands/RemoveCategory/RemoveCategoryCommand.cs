using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.RemoveCategory;

/// <summary>
/// Represents a command to remove a category by ID.
/// </summary>
/// <param name="CategoryId">Category ID.</param>
public sealed record RemoveCategoryCommand(
    Guid CategoryId) : ICommand;