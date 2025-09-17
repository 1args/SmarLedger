namespace SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Models;

/// <summary>
/// Model for creating a delivery attempt record.
/// </summary>
/// <param name="WebhookId">Webhook ID.</param>
/// <param name="Payload">JSON payload that was sent in the webhook request.</param>
/// <param name="ResponseStatusCode">HTTP response status code received from the target endpoint.</param>
/// <param name="IsSuccess">Indicates whether the webhook delivery was successful.</param>
/// <param name="AttemptedAt">Date and time when the delivery attempt occurred.</param>
public sealed record WebhookDeliveryAttemptCreationModel(
    Guid WebhookId,
    string Payload,
    int? ResponseStatusCode,
    bool IsSuccess,
    DateTime AttemptedAt);