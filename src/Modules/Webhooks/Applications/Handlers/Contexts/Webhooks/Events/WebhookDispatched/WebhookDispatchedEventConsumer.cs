using MassTransit;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Abstractions;
using SmartLedger.Modules.Webhooks.Contracts.Events;

namespace SmartLedger.Modules.Webhooks.Applications.Handlers.Contexts.Webhooks.Events.WebhookDispatched;

/// <summary>
/// Consumes the <see cref="WebhookDispatchedEvent"/>.
/// </summary>
public sealed class WebhookDispatchedEventConsumer(
    IWebhooksService webhooksService,
    IBus eventBus,
    ILogger<WebhookDispatchedEventConsumer> logger) : IEventConsumer<WebhookDispatchedEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(WebhookDispatchedEvent @event, CancellationToken cancellationToken)
    {
        var webhooks = await webhooksService.GetWebhooksAsync(@event.EventType, cancellationToken);

        logger.LogInformation("Found {WebhookCount} webhooks for event type {EventType}.",
            webhooks.Count, @event.EventType);

        foreach (var webhook in webhooks)
        {
            await eventBus.Publish(
                new WebhookTriggeredEvent(
                    webhook.Id,
                    webhook.EventType,
                    webhook.CallbackUrl,
                    @event.Data),
                cancellationToken);

            logger.LogInformation("Published WebhookTriggeredEvent for webhook {WebhookId} to {CallbackUrl}.",
                webhook.Id, webhook.CallbackUrl);
        }
    }
}