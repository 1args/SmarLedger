namespace SmartLedger.Modules.Webhooks.Contracts.Responses;

/// <summary>
/// Represents the response for a webhook event.
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <param name="Id"></param>
/// <param name="EventType"></param>
/// <param name="SubscriptionId"></param>
/// <param name="CreatedAt"></param>
/// <param name="Data"></param>
public sealed record WebhookResponse<TData>(
    Guid Id,
    string EventType,
    Guid SubscriptionId,
    DateTime CreatedAt,
    TData Data);