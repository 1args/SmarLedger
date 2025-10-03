namespace SmartLedger.Common.Hosts.RateLimiting;

/// <summary>
/// Options for configuring rate limiting.
/// </summary>
public sealed class RateLimitOptions
{
    /// <summary>Period of time during which a user can make a certain number of requests.</summary>
    public int WindowSizeInSeconds { get; set; }

    /// <summary>Number of max requests.</summary>
    public int MaxRequests { get; set; }
}