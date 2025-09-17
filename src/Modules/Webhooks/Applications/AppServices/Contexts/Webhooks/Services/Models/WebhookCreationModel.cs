namespace SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Models;

/// <summary>
/// Model used to create a new webhook subscription.
/// </summary>
/// <param name="EventType">Event type.</param>
/// <param name="CallbackUrl">Destination URL where the webhook payload will be delivered.</param>
/// <param name="CreatedAt">Date and time the webhook was created.</param>
public sealed record WebhookCreationModel(
    string EventType,
    string CallbackUrl,
    DateTime CreatedAt);