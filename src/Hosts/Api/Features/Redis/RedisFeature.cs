using Microsoft.Extensions.Caching.Hybrid;
using SmartLedger.Common.Hosts.Features.Abstractions;
using HybridCacheOptions = SmartLedger.Common.Contracts.Options.HybridCacheOptions;

namespace SmartLedger.Hosts.Api.Features.Redis;

/// <summary>
/// Feature for configuring Redis as a distributed cache using HybridCache.
/// </summary>
internal class RedisFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        var hybridCacheOptions = configuration.GetSection(nameof(HybridCacheOptions)).Get<HybridCacheOptions>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
        });

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = hybridCacheOptions!.MaximumPayloadBytes;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = hybridCacheOptions.LocalCacheExpirationSeconds,
                Expiration = hybridCacheOptions.DefaultExpirationSeconds
            };
        });
    }
}
