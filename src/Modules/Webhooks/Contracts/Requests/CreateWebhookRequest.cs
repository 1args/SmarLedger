namespace SmartLedger.Modules.Webhooks.Contracts.Requests;

/// <summary>
/// Represents a request to create a new webhook.
/// </summary>
/// <param name="EventType">Event type.</param>
/// <param name="CallbackUrl">Destination URL where the webhook payload will be delivered.</param>
public sealed record CreateWebhookRequest(
    string EventType,
    string CallbackUrl);