namespace SmartLedger.Common.Hosts.RateLimiting.Abstractions;

public interface IRateLimiter
{
    Task<bool> IsAllowedAsync(string ipAddress, string method, string path, CancellationToken cancellationToken);
}