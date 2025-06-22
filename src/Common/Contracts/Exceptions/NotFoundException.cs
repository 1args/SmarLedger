namespace SmartLedger.Common.Contracts.Exceptions;

/// <summary>
/// Thrown when a requested resource is not found.
/// </summary>
public sealed class NotFoundException(string message) : Exception(message);