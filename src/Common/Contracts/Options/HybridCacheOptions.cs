namespace SmartLedger.Common.Contracts.Options;

/// <summary>
/// Options for configuring a hybrid cache system.
/// </summary>
public sealed class HybridCacheOptions
{
    /// <summary>Maximum size of the in-memory cache.</summary>
    public int MaximumPayloadBytes { get; set; }

    /// <summary>Default expiration time for items in the cache.</summary>
    public TimeSpan DefaultExpirationSeconds { get; set; }

    /// <summary>Expiration time for items in the local cache.</summary>
    public TimeSpan LocalCacheExpirationSeconds { get; set; }
}