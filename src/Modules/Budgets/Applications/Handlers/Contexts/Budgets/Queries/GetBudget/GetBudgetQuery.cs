using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Budgets.Contracts.Responses.Budgets;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudget;

/// <summary>
/// Represents a query to retrieve a budget.
/// </summary>
/// <param name="BudgetId">Budget ID.</param>
public sealed record GetBudgetQuery(
    Guid BudgetId) : IQuery<BudgetResponse>;