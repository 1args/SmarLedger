namespace SmartLedger.Common.Hosts.RateLimiting.Abstractions;

/// <summary>
/// Interface used to represent the rate limiter.
/// </summary>
public interface IRateLimiter
{
    /// <summary>
    /// Checks whether a request is allowed according to the configured rate limits.
    /// </summary>
    /// <param name="ipAddress">IP address of the requester.</param>
    /// <param name="method">HTTP method.</param>
    /// <param name="path">Requested path/endpoint.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Returns <c>true</c> if the request is allowed, <c>false</c> otherwise.</returns>
    Task<bool> IsAllowedAsync(string ipAddress, string method, string path, CancellationToken cancellationToken);
}