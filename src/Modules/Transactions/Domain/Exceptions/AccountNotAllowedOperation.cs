namespace SmartLedger.Modules.Transactions.Domain.Exceptions;

/// <summary>
/// Exception thrown when a forbidden operation is attempted on an account.
/// </summary>
public sealed class AccountNotAllowedOperation(string message) : Exception(message);