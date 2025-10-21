using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenTelemetry.Trace;
using System.Net;
using System.Security.Claims;
using SmartLedger.Common.Host.RateLimiting;
using SmartLedger.Common.Host.RateLimiting.Abstractions;
using SmartLedger.Host.Public.Helpers;

namespace SmartLedger.Hosts.Public.Middlewares;

/// <summary>
/// Middleware for handling rate limiting by restricting the number of allowed requests
/// from a specific IP address within a defined time window.
/// </summary>
public sealed class RateLimitingMiddleware(
    IOptions<RateLimitOptions> rateLimitOptions, 
    RequestDelegate next)
{
    private readonly RateLimitOptions _rateLimitOptions = rateLimitOptions.Value;

    /// <summary>Header name used to indicate how long the client should wait before making a new request.</summary>
    private const string RetryAfter = "Retry-After";

    /// <summary>
    /// Invokes the middleware to enforce rate limiting for incoming HTTP requests.
    /// </summary>
    /// <param name="httpContext">HTTP context representing the current request.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var cancellationToken = httpContext.RequestAborted;

        var rateLimiter = httpContext.RequestServices.GetRequiredService<IRateLimiter>();

        var ipAddress = httpContext.Request
            .GetIpAddress()?.ToString()
            ?? "unknown_ip_address";

        var isAllowed = await rateLimiter.IsAllowedAsync(
            ipAddress, httpContext.Request.Method, httpContext.Request.Path, cancellationToken);

        if (!isAllowed)
        {
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            httpContext.Response.Headers[RetryAfter] = _rateLimitOptions.WindowSizeInSeconds.ToString();

            var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
            var problemDetails = CreateProblemDetails(httpContext, _rateLimitOptions.WindowSizeInSeconds);

            await problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext 
                { 
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails
                });

            return;
        }

        await next(httpContext);
    }

    /// <summary>
    /// Creates <see cref="ProblemDetails"/> object describing the rate limiting error.
    /// </summary>
    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, int seconds)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)StatusCodes.Status429TooManyRequests,
            Title = "Too Many Requests",
            Detail = $"You have sent too many requests in a given amount of time. Please try again later after {seconds} seconds.",
            Instance = httpContext.Request.Path
        };

        return problemDetails;
    }
}