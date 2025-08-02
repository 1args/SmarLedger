namespace SmartLedger.Modules.Security.Contracts.Exceptions;

/// <summary>
/// Represents an exception that occurs during Keycloak API client operations.
/// </summary>
public sealed class KeycloakApiException(string message) : Exception(message);