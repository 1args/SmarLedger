using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;

/// <summary>
/// Filters budget items that are active based on a specific date.
/// </summary>
/// <param name="dateTime">Date time.</param>
public sealed class ActiveBudgetCategorySpecification(DateTime dateTime)
    : Specification<BudgetCategoryReadModel>(bi => bi.StartDate <= dateTime && bi.EndDate >= dateTime);