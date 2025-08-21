using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructures.DataAccess.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

/// <summary>
/// Filters transactions by category if specified.
/// </summary>
/// <param name="category">Optional category to filter transactions by.</param>
public sealed class TransactionByCategorySpecification(string? category)
    : Specification<TransactionReadModel>(t => string.IsNullOrEmpty(category) || t.Category == category);