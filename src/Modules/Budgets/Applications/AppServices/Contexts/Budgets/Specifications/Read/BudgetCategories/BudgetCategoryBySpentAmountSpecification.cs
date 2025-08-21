using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget categories by a spent amount range.
/// </summary>
/// <param name="minSpentAmount">Optional minimum spent amount.</param>
/// <param name="maxSpentAmount">Optional maximum spent amount.</param>
public sealed class BudgetCategoryBySpentAmountSpecification(decimal? minSpentAmount, decimal? maxSpentAmount)
    : Specification<BudgetCategoryReadModel>(
        bc => (!minSpentAmount.HasValue || bc.SpentAmount >= minSpentAmount.Value) &&
              (!maxSpentAmount.HasValue || bc.SpentAmount <= maxSpentAmount.Value));