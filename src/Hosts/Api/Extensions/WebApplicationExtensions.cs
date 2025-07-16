using Scalar.AspNetCore;
using SmartLedger.Hosts.Api.Endpoints;

namespace SmartLedger.Hosts.Api.Extensions;

/// <summary>
/// Extension for configuring middlewares in a <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures http request pipeline.
    /// </summary>
    /// <param name="app">Current <see cref="WebApplication"/> instance.</param>
    /// <returns><see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.MapApiEndpoints();

        return app;
    }

    /// <summary>
    /// Registers Api endpoints.
    /// </summary>
    private static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapAccountsEndpoints()
            .MapTransactionsEndpoints()
            .MapBudgetsEndpoints();

        return endpoints;
    }
}
