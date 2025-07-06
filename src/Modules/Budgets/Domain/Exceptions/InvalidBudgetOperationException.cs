using SmartLedger.Common.Domain.Exceptions;

namespace SmartLedger.Modules.Budgets.Domain.Exceptions;

/// <summary>
/// Exception is thrown when an invalid budged operation is performed.
/// </summary>
public sealed class InvalidBudgetOperationException(string propertyName, string message)
    : DomainException(propertyName, message);