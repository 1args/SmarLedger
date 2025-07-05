using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

/// <summary>
/// Filters transactions by type if specified.
/// </summary>
/// <param name="type">Optional transaction type (e.g., "Income", "Expense").</param>
public sealed class TransactionByTypeSpecification(string? type)
    : Specification<TransactionReadModel>(t => string.IsNullOrEmpty(type) || t.Type == type);