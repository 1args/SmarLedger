using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.BankAccounts.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.BankAccounts.Applications.AppServices.Contexts.Accounts.Specifications.Transactions;

/// <summary>
/// Filters transactions by a date range based on creation date.
/// </summary>
/// <param name="startDate">Optional start date (inclusive).</param>
/// <param name="endDate">Optional end date (inclusive).</param>
public sealed class TransactionByDateRangeSpecification(DateTime? startDate, DateTime? endDate)
    : Specification<TransactionReadModel>(
        t => (!startDate.HasValue || t.CreatedAt >= startDate.Value) &&
            (!endDate.HasValue || t.CreatedAt <= endDate.Value));