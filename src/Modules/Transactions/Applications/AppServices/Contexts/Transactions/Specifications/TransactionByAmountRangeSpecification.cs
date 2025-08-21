using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

/// <summary>
/// Filters transactions by a minimum and/or maximum amount.
/// </summary>
/// <param name="minAmount">Optional minimum transaction amount.</param>
/// <param name="maxAmount">Optional maximum transaction amount.</param>
public sealed class TransactionByAmountRangeSpecification(decimal? minAmount, decimal? maxAmount)
    : Specification<TransactionReadModel>(
        t => (!minAmount.HasValue || t.Amount >= minAmount.Value) &&
             (!maxAmount.HasValue || t.Amount <= maxAmount.Value));