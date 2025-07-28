namespace SmartLedger.Hosts.Api.Features.OpenTelemetry;

/// <summary>
/// Options for configuring OpenTelemetry features.
/// </summary>
public class OpenTelemetryFeatureOptions
{
    /// <summary>Name of the application for which OpenTelemetry is configured.</summary>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>URL for the OpenTelemetry Protocol (OTLP) exporter.</summary>
    public string OtlpEporterUrl { get; set; } = string.Empty;
}
