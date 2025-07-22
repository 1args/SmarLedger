using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Domain.Aggregates;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Write.Budgets;

/// <summary>
/// Filters budgets that are active based on a specific date.
/// </summary>
/// <param name="dateTime">Date time.</param>
public sealed class ActiveBudgetSpecification(DateTime dateTime)
    : Specification<Budget>(b => b.Period.StartDate <= dateTime && b.Period.EndDate >= dateTime);