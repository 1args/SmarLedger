using Scalar.AspNetCore;

namespace SmartLedger.Hosts.Api.Extensions;

/// <summary>
/// Extension for configuring middlewares in a <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures API middlewares for the application.
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

        return app;
    }
}
