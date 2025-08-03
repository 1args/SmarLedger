namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents the discovery document for Keycloak.
/// </summary>
public sealed class DiscoveryDocument
{
    /// <summary>URL for receiving tokens.</summary>
    public string TokenEndpoint { get; set; } = string.Empty;

    // Endpoints can be expanded as needed.
}