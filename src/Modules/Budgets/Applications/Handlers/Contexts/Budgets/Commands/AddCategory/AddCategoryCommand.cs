using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Common.Domain.Enums;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Commands.AddCategory;

/// <summary>
/// Represents a command to add a new category (budget item) to a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
/// <param name="Category">Transaction category.</param>
/// <param name="Limit">Spending limit for the category.</param>
public sealed record AddCategoryCommand(
    Guid BudgetId,
    FinancialCategory Category,
    decimal Limit) : ICommand<Guid>;