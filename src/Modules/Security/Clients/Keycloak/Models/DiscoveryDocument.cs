namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents the discovery document for Keycloak.
/// </summary>
public class DiscoveryDocument
{
    /// <summary>URL for receiving tokens.</summary>
    public string TokenEndpoint { get; set; } = string.Empty;

    /// <summary>URL for logging out of the session.</summary>
    public string LogoutEndpoint { get; set; } = string.Empty;
}