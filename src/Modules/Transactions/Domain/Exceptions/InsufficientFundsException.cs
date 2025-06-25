using SmartLedger.Common.Domain.Exceptions;

namespace SmartLedger.Modules.Transactions.Domain.Exceptions;

/// <summary>
/// Exception is thrown when there are insufficient funds for a transaction.
/// </summary>
public sealed class InsufficientFundsException(string propertyName, string message)
    : DomainException(propertyName, message);