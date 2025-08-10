namespace SmartLedger.Modules.Security.Contracts.Options;

/// <summary>
/// Options for configuring Keycloak authorization.
/// </summary>
public sealed class KeycloakAuthorizationOptions
{
    /// <summary>Keycloak client ID.</summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>Keycloak client UUID.</summary>
    public string ClientUuid { get; set; } = string.Empty;

    /// <summary>Keycloak client secret.</summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>Keycloak realm name.</summary>
    public string Realm { get; set; } = string.Empty;

    /// <summary>Keycloak metadata address, used for OpenID Connect discovery and configuration.</summary>
    public string MetadataAddress { get; set; } = string.Empty;

    /// <summary>Keycloak admin client ID.</summary>
    public string AdminClient { get; set; } = string.Empty;

    /// <summary>Keycloak admin secret.</summary>
    public string AdminSecret { get; set; } = string.Empty;

    /// <summary>Keycloak admin base URL.</summary>
    public string AdminBaseUrl { get; set; } = string.Empty;

    /// <summary>Keycloak admin realm name.</summary>
    public string AdminRealm { get; set; } = string.Empty;

    /// <summary>Keycloak server URL.</summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>Keycloak authorization URL.</summary>
    public string AuthorizationUrl { get; set; } = string.Empty;

    /// <summary>Options for validating JWT tokens.</summary>
    public TokenValidationOptions TokenValidationOptions { get; set; } = new();
}