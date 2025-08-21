using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget items by a specific category.
/// </summary>
/// <param name="category">Category.</param>
public sealed class BudgetCategoryByCategorySpecification(string category)
    : Specification<BudgetCategoryReadModel>(bc => bc.Category == category);