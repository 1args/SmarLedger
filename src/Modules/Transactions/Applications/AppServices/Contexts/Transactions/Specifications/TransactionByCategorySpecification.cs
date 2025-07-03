using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

public sealed class TransactionByCategorySpecification(string? category)
    : Specification<TransactionReadModel>(t => string.IsNullOrEmpty(category) || t.Category == category);