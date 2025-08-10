using Microsoft.Extensions.Caching.Hybrid;
using SmartLedger.Common.Infrastructure.Abstractions;

namespace SmartLedger.Common.Infrastructure.Caching;

/// <summary>
/// Hybrid cache class for caching data across distributed and local caches.
/// </summary>
public sealed class DefaultHybridCache(HybridCache hybridCache) : IHybridCache
{
    /// <inheritdoc/>
    public async ValueTask<TData> GetOrCreateAsync<TData>(string key, Func<CancellationToken, ValueTask<TData>> factory, CancellationToken cancellationToken,
        HybridCacheEntryOptions? options = null)
    {
        return await hybridCache.GetOrCreateAsync(key, factory, options, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask SetAsync<TData>(string key, TData data, CancellationToken cancellationToken,
        HybridCacheEntryOptions? options = null)
    {
        await hybridCache.SetAsync(key, data, options, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await hybridCache.RemoveAsync(key, cancellationToken);
    }
}