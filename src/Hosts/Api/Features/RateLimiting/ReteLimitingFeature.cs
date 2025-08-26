using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Hosts.Api.Features.RateLimiting;

/// <summary>
/// Feature for configuring rate limiting.
/// </summary>
internal class ReteLimitingFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds}";

                    var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ReteLimitingFeature>>();

                    var problemDetails = problemDetailsFactory.CreateProblemDetails(
                        context.HttpContext,
                        StatusCodes.Status429TooManyRequests,
                        "Too Many Requests",
                        detail: $"Too many requests. Please try again after {retryAfter.TotalSeconds} seconds.");

                    logger.LogWarning(
                        "Request rejected for IP {RemoteIp} on {Path} due to rate limiting. Retry after {RetryAfter} seconds",
                        context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                        context.HttpContext.Request.Path,
                        retryAfter.TotalSeconds);

                    await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                }
            };

            options.AddPolicy<string>(RateLimitPolicy.IpAddress, httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromMinutes(1),
                        PermitLimit = 20,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    })!;
            });

            options.AddPolicy<string>(RateLimitPolicy.ReportGeneration, httpContext =>
            {
                return RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: httpContext.User.Identity?.Name 
                                  ?? httpContext.Connection.RemoteIpAddress?.ToString() 
                                  ?? "anonymous",
                    factory: _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 1
                    })!;
            });

            options.AddSlidingWindowLimiter(RateLimitPolicy.ReadOperations, limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromSeconds(30);
                limiterOptions.PermitLimit = 100;
                limiterOptions.SegmentsPerWindow = 10;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 5;
            });

            options.AddTokenBucketLimiter(RateLimitPolicy.WriteOperations, limiterOptions =>
            {
                limiterOptions.TokenLimit = 30;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 3;
                limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(20);
                limiterOptions.TokensPerPeriod = 20;
                limiterOptions.AutoReplenishment = true;
            });

            options.AddFixedWindowLimiter(RateLimitPolicy.Authentication, limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.PermitLimit = 5;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0;
            });

            options.AddSlidingWindowLimiter(RateLimitPolicy.SearchOperations, limiterOptions =>
            {
                limiterOptions.Window = TimeSpan.FromSeconds(45);
                limiterOptions.PermitLimit = 30;
                limiterOptions.SegmentsPerWindow = 6;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 5;
            });

            options.AddConcurrencyLimiter(RateLimitPolicy.Global, limiterOptions =>
            {
                limiterOptions.PermitLimit = 50;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 15;
            });
        });
    }
}