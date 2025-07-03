using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

public sealed class TransactionByDateRangeSpecification(DateTime? startDate, DateTime? endDate)
    : Specification<TransactionReadModel>(
        t => (!startDate.HasValue || t.CreatedAt >= startDate.Value) &&
            (!endDate.HasValue || t.CreatedAt <= endDate.Value));