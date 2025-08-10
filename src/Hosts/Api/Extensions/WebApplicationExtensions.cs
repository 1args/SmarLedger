using Scalar.AspNetCore;
using SmartLedger.Hosts.Api.Endpoints;
using SmartLedger.Hosts.Api.Middlewares;

namespace SmartLedger.Hosts.Api.Extensions;

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
            application.MapScalarApiReference();
        }

        application.UseHttpsRedirection();
        application.UseAuthentication();
        application.UseAuthorization();
        application.UseMiddleware<AuthorizationMiddleware>();
        application.MapApiEndpoints();

        return application;
    }

    /// <summary>
    /// Registers Api endpoints.
    /// </summary>
    private static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapAccountsEndpoints()
            .MapTransactionsEndpoints()
            .MapBudgetsEndpoints()
            .MapIdentifyEndpoints();

        return endpoints;
    }
}
