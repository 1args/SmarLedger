using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Budgets.Contracts.Responses.BudgetCategories;

namespace SmartLedger.Modules.Budgets.Applications.Handlers.Contexts.Budgets.Queries.GetBudgetCategory;

/// <summary>
/// Represents a query to retrieve a budget.
/// </summary>
/// <param name="BudgetCategoryId">Budget category ID.</param>
public sealed record GetBudgetCategoryQuery(
    Guid BudgetCategoryId) : IQuery<BudgetCategoryResponse>;