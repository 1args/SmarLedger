using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Accounts.Specifications;

/// <summary>
/// Filters accounts by a date range based on their creation date.
/// </summary>
/// <param name="startDate">Optional start date (inclusive).</param>
/// <param name="endDate">Optional end date (inclusive).</param>
public sealed class AccountByDateRangeSpecification(DateTime? startDate, DateTime? endDate)
    : Specification<AccountReadModel>(
        a => (!startDate.HasValue || a.CreatedAt >= startDate.Value) && 
             (!endDate.HasValue || a.CreatedAt <= endDate.Value));