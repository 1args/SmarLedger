using SmartLedger.Common.Hosts.Features.Abstractions;
using SmartLedger.Common.Hosts.RateLimiting;
using SmartLedger.Common.Hosts.RateLimiting.Abstractions;

namespace SmartLedger.Hosts.Api.Features.RateLimiter;

/// <summary>
/// Feature for addition rate limiter.
/// </summary>
internal class RateLimiterFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRateLimiter, DefaultRateLimiter>();
    }
}