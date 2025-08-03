namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents an active session of a user.
/// </summary>
/// <param name="Id">Unique identifier of the session.</param>
/// <param name="StartDateTime">Date time when the session was started.</param>
/// <param name="LastAccessDateTime">Date time the user interacted with the system during this session.</param>
/// <param name="ClientIpAddress">IP address of the client that initiated the session.</param>
public sealed record KeycloakUserSessionResponse(
    Guid Id,
    DateTime? StartDateTime,
    DateTime? LastAccessDateTime,
    string ClientIpAddress);