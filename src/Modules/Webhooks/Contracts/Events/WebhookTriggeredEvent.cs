using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Webhooks.Contracts.Events;

/// <summary>
/// Event triggered when a webhook is triggered.
/// </summary>
/// <param name="WebhookId">Webhook ID.</param>
/// <param name="EventType">Event type.</param>
/// <param name="CallbackUrl">Webhook URL.</param>
/// <param name="Data">Data that will be dispatched.</param>
public sealed record WebhookTriggeredEvent(
    Guid WebhookId,
    string EventType,
    string CallbackUrl,
    object Data) : Event;