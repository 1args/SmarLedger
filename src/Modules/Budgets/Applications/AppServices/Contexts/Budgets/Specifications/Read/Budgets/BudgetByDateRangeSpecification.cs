using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.Budgets;

/// <summary>
/// Filters budgets by a date range.
/// </summary>
/// <param name="startDate">Start date.</param>
/// <param name="endDate">End date.</param>
public sealed class BudgetByDateRangeSpecification(DateTime? startDate, DateTime? endDate)
    : Specification<BudgetReadModel>(
        b => (!startDate.HasValue || b.StartDate >= startDate.Value) &&
             (!endDate.HasValue || b.EndDate <= endDate.Value));