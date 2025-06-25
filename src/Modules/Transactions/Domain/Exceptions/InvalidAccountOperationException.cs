using SmartLedger.Common.Domain.Exceptions;

namespace SmartLedger.Modules.Transactions.Domain.Exceptions;

/// <summary>
/// Exception is thrown when an invalid account operation is performed.
/// </summary>
public sealed class InvalidAccountOperationException(string propertyName, string message)
    : DomainException(propertyName, message);