using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget categories by a limit range.
/// </summary>
/// <param name="minLimit">Optional minimum limit.</param>
/// <param name="maxLimit">Optional maximum limit.</param>
public sealed class BudgetCategoryByLimitRangeSpecification(decimal? minLimit, decimal? maxLimit)
    : Specification<BudgetCategoryReadModel>(
        bc => (!minLimit.HasValue || bc.Limit >= minLimit.Value) &&
              (!maxLimit.HasValue || bc.Limit <= maxLimit.Value));
