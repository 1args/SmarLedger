namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

/// <summary>
/// Represents an attempt to deliver a webhook to its destination.
/// </summary>
public sealed class WebhookDeliveryAttempt
{
    /// <summary>Unique identifier of the webhook delivery attempt.</summary>
    public Guid Id { get; set; }

    /// <summary>Webhook ID.</summary>
    public Guid WebhookId { get; set; }

    /// <summary>JSON payload that was sent in the webhook request.</summary>
    public string Payload { get; set; } = string.Empty;

    /// <summary>HTTP response status code received from the target endpoint.</summary>
    public int? ResponseStatusCode { get; set; }

    /// <summary>Indicates whether the webhook delivery was successful.</summary>
    public bool IsSuccess { get; set; }

    /// <summary>Date and time when the delivery attempt occurred.</summary>
    public DateTime AttemptedAt { get; set; }
}