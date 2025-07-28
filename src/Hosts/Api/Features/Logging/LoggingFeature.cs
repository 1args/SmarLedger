using Serilog;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Hosts.Api.Features.Logging;

/// <summary>
/// Feature for configuring logging using Serilog.
/// </summary>
internal class LoggingFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(logger, dispose: true);
        });
    }
}
