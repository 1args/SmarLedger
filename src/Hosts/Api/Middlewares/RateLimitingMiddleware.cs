using HandlebarsDotNet;
using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;
using SmartLedger.Common.Hosts.RateLimiting;
using SmartLedger.Common.Hosts.RateLimiting.Abstractions;
using SmartLedger.Hosts.Api.Helpers;
using System.Security.Claims;

namespace SmartLedger.Hosts.Api.Middlewares;

public sealed class RateLimitingMiddleware(RequestDelegate next)
{
    private static readonly string RetryAfter = "Retry-After";

    public async Task InvokeAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var rateLimiter = httpContext.RequestServices.GetRequiredService<IRateLimiter>();
        var options = httpContext.RequestServices.GetRequiredService<IOptions<RateLimitOptions>>().Value;

        var ipAddress = IpAddressHelper
            .GetIpAddress(httpContext.Request)?.ToString()
            ?? "unknown_ip_address";

        var isAllowed = await rateLimiter.IsAllowedAsync(
            ipAddress, httpContext.Request.Method, httpContext.Request.Path, cancellationToken);

        if(!isAllowed)
        {
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            httpContext.Response.Headers[RetryAfter] = options.WindowSizeInSeconds.ToString();
            await httpContext.Response.WriteAsync("429 Too Many Requests");
            return;
        }

        await next(httpContext);
    }
}