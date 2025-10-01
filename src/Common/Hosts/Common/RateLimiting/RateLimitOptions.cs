namespace SmartLedger.Common.Hosts.RateLimiting;

public sealed class RateLimitOptions
{
    public int WindowSizeInSeconds { get; set; }

    public int MaxRequests { get; set; }
}