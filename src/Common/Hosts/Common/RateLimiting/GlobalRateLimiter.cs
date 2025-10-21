using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using SmartLedger.Common.Host.RateLimiting.Abstractions;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;

namespace SmartLedger.Common.Host.RateLimiting;

/// <summary>
/// Class used to represent the rate limiter.
/// </summary>
public sealed class GlobalRateLimiter(
    IHybridCache hybridCache,
    IOptions<RateLimitOptions> rateLimitOptions) : IRateLimiter
{
    private readonly RateLimitOptions _rateLimitOptions = rateLimitOptions.Value;

    /// <inheritdoc/>
    public async Task<bool> IsAllowedAsync(string ipAddress, string method, string path, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(ipAddress))
        {
            return false;
        }

        var (currentWindow, previousWindow) = GetCurrentAndPreviousWindows();

        var currentKey = GenerateKey(ipAddress, method, path, currentWindow);
        var previousKey = GenerateKey(ipAddress, method, path, previousWindow);

        var currentCount = await GetOrCreateCounterAsync(currentKey, cancellationToken);
        var previousCount = await GetOrCreateCounterAsync(previousKey, cancellationToken);

        var overlapWeight = GetOverlapWeight();

        // Effective number of requests = current requests + (previous requests * overlap weight)
        var effectiveCount = currentCount + (int)(previousCount * overlapWeight);

        if(effectiveCount >= _rateLimitOptions.MaxRequests)
        {
            return false;
        }

        await IncrementCounterAsync(currentKey, cancellationToken);

        return true;
    }

    private async Task<int> GetOrCreateCounterAsync(string key, CancellationToken cancellationToken)
    {
        return await hybridCache.GetOrCreateAsync<int>(
            key,
            (ct) => ValueTask.FromResult(0),
            cancellationToken,
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromSeconds(_rateLimitOptions.WindowSizeInSeconds * 2)
            });
    }

    private async Task IncrementCounterAsync(string key, CancellationToken cancellationToken)
    {
        var currentCount = await GetOrCreateCounterAsync(key, cancellationToken);
        await hybridCache.SetAsync(
            key,
            currentCount + 1,
            cancellationToken,
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromSeconds(_rateLimitOptions.WindowSizeInSeconds * 2)
            });
    }

    private string GenerateKey(string ipAddress, string method, string path, long window) =>
         $"rate_limit:{ipAddress}:{method}:{path}:{window}";

    /// <summary>
    /// Calculates the current and previous window numbers based on the current time.
    /// </summary>
    private (long currentWindow, long previousWindow) GetCurrentAndPreviousWindows()
    {
        var windowSizeInSeconds = _rateLimitOptions.WindowSizeInSeconds;
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var currentWindow = currentTime / windowSizeInSeconds; // Page number
        var previousWindow = currentWindow - 1;

        return (currentWindow, previousWindow);
    }

    /// <summary>
    /// Calculates the weight of the previous window based on how much time has elapsed
    /// in the current window.
    /// </summary>
    private double GetOverlapWeight()
    {
        var windowSizeInSeconds = _rateLimitOptions.WindowSizeInSeconds;
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var currentWindowStart = (currentTime / windowSizeInSeconds) * windowSizeInSeconds;
        var timeElapsedInCurrentWindow = currentTime - currentWindowStart;

        // Weight = proportion of time remaining in the previous window
        // The more time that has passed in the current window, the less influence the previous window has
        var weight = 1.0 - ((double)timeElapsedInCurrentWindow / windowSizeInSeconds);

        return Math.Max(0, Math.Min(1, weight));
    }
}