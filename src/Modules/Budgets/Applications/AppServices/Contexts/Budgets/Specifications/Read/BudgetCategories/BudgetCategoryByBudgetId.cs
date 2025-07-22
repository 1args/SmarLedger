using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget categories by a specific budget ID.
/// </summary>
/// <param name="budgetId">Budget ID></param>
public sealed class BudgetCategoryByBudgetId(Guid budgetId)
    : Specification<BudgetCategoryReadModel>(bc => bc.BudgetId == budgetId);