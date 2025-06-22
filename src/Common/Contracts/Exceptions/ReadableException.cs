namespace SmartLedger.Common.Contracts.Exceptions;

/// <summary>
/// Thrown when a server error occurs and informs what went wrong for more specifics.
/// </summary>
public sealed class ReadableException(string message) : Exception(message);