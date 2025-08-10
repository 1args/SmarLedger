using System.Text.Json.Serialization;

namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents the discovery document for Keycloak.
/// </summary>
public sealed record DiscoveryDocument
{
    /// <summary>URL for receiving tokens.</summary>
    [JsonPropertyName("token_endpoint")]
    public string TokenEndpoint { get; init; } = string.Empty;

    // Endpoints can be expanded as needed.
}