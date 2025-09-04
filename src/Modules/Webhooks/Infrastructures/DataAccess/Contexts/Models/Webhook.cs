namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

/// <summary>
/// Represents a webhook subscription entity.
/// </summary>
public sealed class Webhook
{
    /// <summary>Unique identifier of the webhook subscription.</summary>
    public Guid Id { get; set; }

    /// <summary>Type of the event that triggers the webhook.</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>Destination URL where the webhook payload will be delivered.</summary>
    public string CallbackUrl { get; set; } = string.Empty;

    /// <summary>Date and time the webhook was created.</summary>
    public DateTime CreatedAt { get; set; }
}