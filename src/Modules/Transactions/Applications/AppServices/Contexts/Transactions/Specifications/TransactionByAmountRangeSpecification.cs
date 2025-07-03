using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

public sealed class TransactionByAmountRangeSpecification(decimal? minAmount, decimal? maxAmount)
    : Specification<TransactionReadModel>(
        t => (!minAmount.HasValue || t.Amount >= minAmount.Value) &&
             (!maxAmount.HasValue || t.Amount <= maxAmount.Value));