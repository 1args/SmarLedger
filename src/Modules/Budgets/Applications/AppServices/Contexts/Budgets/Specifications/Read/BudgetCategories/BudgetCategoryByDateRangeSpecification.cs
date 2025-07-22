using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Budgets.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Budgets.Applications.AppServices.Contexts.Budgets.Specifications.Read.BudgetCategories;

/// <summary>
/// Filters budget categories by a date range.
/// </summary>
/// <param name="startDate">Optional tart date.</param>
/// <param name="endDate">Optional end date.</param>
public sealed class BudgetCategoryByDateRangeSpecification(DateTime? startDate, DateTime? endDate)
    : Specification<BudgetCategoryReadModel>(
        bc => (!startDate.HasValue || bc.StartDate >= startDate.Value) &&
             (!endDate.HasValue || bc.EndDate <= endDate.Value));