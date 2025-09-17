using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Webhooks.Contracts.Events;

/// <summary>
/// Event triggered when a webhook is dispatched.
/// </summary>
/// <param name="EventType">Event type.</param>
/// <param name="Data">Data that will be dispatched.</param>
public sealed record WebhookDispatchedEvent(
    string EventType,
    object Data) : Event;