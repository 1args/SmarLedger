using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Infrastructure.Contexts.Read.Models;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

public sealed class TransactionByTypeSpecification(string? type)
    : Specification<TransactionReadModel>(t => string.IsNullOrEmpty(type) || t.Type == type);