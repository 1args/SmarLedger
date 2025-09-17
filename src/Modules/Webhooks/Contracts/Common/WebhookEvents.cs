namespace SmartLedger.Modules.Webhooks.Contracts.Common;

/// <summary>
/// Provides constants for webhook event names used to identify specific event types in webhook payloads.
/// </summary>
public sealed class WebhookEvents
{
    /// <summary> Represents the event name used to indicate that a report has been generated. </summary>
    public const string ReportGenerated = "report.generated";
}