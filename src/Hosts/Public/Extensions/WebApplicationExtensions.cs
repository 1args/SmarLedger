using Hangfire;
using Scalar.AspNetCore;
using SmartLedger.Host.Public.Endpoints;
using SmartLedger.Hosts.Public.Middlewares;

namespace SmartLedger.Hosts.Public.Extensions;

/// <summary>
/// Extension for configuring middlewares in a <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures http request pipeline.
    /// </summary>
    /// <param name="application">Current <see cref="WebApplication"/> instance.</param>
    /// <returns><see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseApiMiddlewares(this WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.MapOpenApi();
            application.MapScalarApiReference(options =>
            {
                options.Title = "SmartLedger API";
            });
        }

        application.UseHttpsRedirection();
        application.UseAuthentication();
        application.UseAuthorization();
        application.UseExceptionHandler();
        application.UseMiddleware<AuthorizationMiddleware>();
        application.UseMiddleware<RateLimitingMiddleware>();
        application.MapApiEndpoints();
        application.UseHangfireDashboard();

        return application;
    }

    /// <summary>
    /// Registers Api endpoints.
    /// </summary>
    private static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapAccountsEndpoints()
            .MapBudgetsEndpoints()
            .MapIdentifyEndpoints()
            .MapUsersEndpoints()
            .MapReportsEndpoints()
            .MapWebhooksEndpoints();

        return endpoints;
    }
}
