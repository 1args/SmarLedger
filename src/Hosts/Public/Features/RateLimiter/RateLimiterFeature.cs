using SmartLedger.Common.Host.Features.Abstractions;
using SmartLedger.Common.Host.RateLimiting;
using SmartLedger.Common.Host.RateLimiting.Abstractions;

namespace SmartLedger.Host.Public.Features.RateLimiter;

/// <summary>
/// Feature for addition rate limiter.
/// </summary>
internal class RateLimiterFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRateLimiter, GlobalRateLimiter>();
    }
}