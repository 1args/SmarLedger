using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SmartLedger.Common.Hosts.Features.Abstractions;

namespace SmartLedger.Hosts.Api.Features.OpenTelemetry;

/// <summary>
/// Feature for configuring OpenTelemetry for tracing and metrics.
/// </summary>
internal class OpenTelemetryFeature : IAppFeature
{
    /// <inheritdoc />
    public void UseFeature(IServiceCollection services, IConfiguration configuration)
    {
        var openTelemetryOptions = configuration
            .GetSection(nameof(OpenTelemetryFeatureOptions))
            .Get<OpenTelemetryFeatureOptions>();

        if (string.IsNullOrWhiteSpace(openTelemetryOptions?.ApplicationName))
        {
            throw new ArgumentException(
                "OpenTelemetry application name must be provided in the configuration.",
                nameof(openTelemetryOptions.ApplicationName));
        }

        if (string.IsNullOrWhiteSpace(openTelemetryOptions?.OtlpEporterUrl))
        {
            throw new ArgumentException(
                "OpenTelemetry exporter URL must be provided in the configuration.",
                nameof(openTelemetryOptions.OtlpEporterUrl));
        }

        services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => resourceBuilder
                .AddService(openTelemetryOptions.ApplicationName))
            .WithMetrics(meterProviderBuilder => meterProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(openTelemetryOptions.OtlpEporterUrl);
                }))
            .WithTracing(tracingProviderBuilder =>
                tracingProviderBuilder
                    .AddSource(openTelemetryOptions.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(openTelemetryOptions.OtlpEporterUrl);
                    }));

        services.Configure<OpenTelemetryLoggerOptions>(logger => logger.AddOtlpExporter())
            .ConfigureOpenTelemetryMeterProvider(meter => meter.AddOtlpExporter())
            .ConfigureOpenTelemetryTracerProvider(tracer => tracer.AddOtlpExporter());

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.ParseStateValues = true;
                options.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(openTelemetryOptions.OtlpEporterUrl);
                });
                options.AddConsoleExporter();
            });
        });
    }
}
