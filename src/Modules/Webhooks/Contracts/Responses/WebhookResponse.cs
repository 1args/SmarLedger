namespace SmartLedger.Modules.Webhooks.Contracts.Responses;

/// <summary>
/// Represents the response for a webhook event.
/// </summary>
/// <param name="Id">Response ID.</param>
/// <param name="WebhookId">Webhook ID.</param>
/// <param name="EventType">Event type.</param>
/// <param name="CreatedAt">Date ant time the webhook created.</param>
/// <param name="Data">Webhook data.</param>
public sealed record WebhookResponse(
    Guid Id,
    Guid WebhookId,
    string EventType,
    DateTime CreatedAt,
    object Data);