using MassTransit;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Webhooks.Contracts.Events;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Abstractions;
using System.Data;

namespace SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Dispatchers;

/// <inheritdoc />
public sealed class WebhooksDispatcher(
    IBus eventBus,
    ILogger<WebhooksDispatcher> logger) : IWebhooksDispatcher
{
    /// <inheritdoc />
    public async Task DispatchAsync<TData>(string eventType, TData data, CancellationToken cancellationToken) 
        where TData : notnull
    {
        logger.LogInformation("Dispatching webhook event of type {EventType}", eventType);

        await eventBus.Publish(new WebhookDispatchedEvent(eventType, data), cancellationToken);

        logger.LogInformation("Dispatched webhook event of type {EventType}", eventType);
    }
}