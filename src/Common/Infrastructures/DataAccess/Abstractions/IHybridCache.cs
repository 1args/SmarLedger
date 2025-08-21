using Microsoft.Extensions.Caching.Hybrid;

namespace SmartLedger.Common.Infrastructures.DataAccess.Abstractions;

/// <summary>
/// Defines a hybrid cache interface for caching data across distributed and local caches.
/// </summary>
public interface IHybridCache
{
    /// <summary>
    /// Retrieves or creates a cache entry.
    /// </summary>
    /// <typeparam name="TData">Type of the cached data.</typeparam>
    /// <param name="key">Cache key to retrieve or create.</param>
    /// <param name="factory">Factory function to generate the data if it's not already cached.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <param name="options">Optional cache entry settings such as expiration policy.</param>
    /// <returns>Cached or newly created data.</returns>
    ValueTask<TData> GetOrCreateAsync<TData>(string key, Func<CancellationToken, ValueTask<TData>> factory, CancellationToken cancellationToken,
        HybridCacheEntryOptions? options = null);


    /// <summary>
    /// Sets a cache entry with the specified key and data.
    /// </summary>
    /// <typeparam name="TData">Type of the cached data.</typeparam>
    /// <param name="key">Cache key to assign.</param>
    /// <param name="data">Data to store in the cache.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <param name="options">Optional cache entry settings such as expiration policy.</param>
    ValueTask SetAsync<TData>(string key, TData data, CancellationToken cancellationToken, HybridCacheEntryOptions? options = null);

    /// <summary>
    /// Removes a cache entry by its key.
    /// </summary>
    /// <param name="key">The cache key to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    ValueTask RemoveAsync(string key, CancellationToken cancellationToken);
}