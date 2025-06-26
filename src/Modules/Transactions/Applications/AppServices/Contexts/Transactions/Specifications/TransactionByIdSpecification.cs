using SmartLedger.Common.Applications.AppServices.Specifications;
using SmartLedger.Modules.Transactions.Domain.Entities;

namespace SmartLedger.Modules.Transactions.Applications.AppServices.Contexts.Transactions.Specifications;

public sealed class TransactionByIdSpecification(
    Guid transactionId)
    : Specification<Transaction>(t => t.Id == transactionId);