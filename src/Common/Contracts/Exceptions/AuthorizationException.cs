namespace SmartLedger.Common.Contracts.Exceptions;

/// <summary>
/// Thrown during authorization problems.
/// </summary>
/// <param name="message"></param>
public sealed class AuthorizationException(string message) : Exception(message);