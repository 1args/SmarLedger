using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Models;
using SmartLedger.Modules.Webhooks.Contracts.Events;
using SmartLedger.Modules.Webhooks.Contracts.Responses;

namespace SmartLedger.Modules.Webhooks.Host.Consumers.Consumers;

/// <summary>
/// Consumes the <see cref="WebhookTriggeredEvent"/>.
/// </summary>
public sealed class WebhookTriggeredEventConsumer(
    IWebhooksService webhooksService,
    IHttpClientFactory httpClientFactory,
    IDateTimeProvider dateTimeProvider,
    ILogger<WebhookTriggeredEventConsumer> logger) : IEventConsumer<WebhookTriggeredEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(WebhookTriggeredEvent @event, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var payload = new WebhookResponse(
            Guid.NewGuid(),
            @event.WebhookId,
            @event.EventType,
            dateTimeProvider.UtcNow,
            @event.Data);

        var jsonPayload = JsonSerializer.Serialize(payload);

        var response = await httpClient.PostAsJsonAsync(@event.CallbackUrl, payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        logger.LogInformation("Webhook {WebhookId} dispatched to {CallbackUrl} with status code {StatusCode}.",
            @event.WebhookId, @event.CallbackUrl, response.StatusCode);

        try
        {
            var request = new WebhookDeliveryAttemptCreationModel(
                @event.WebhookId,
                jsonPayload,
                (int)response.StatusCode,
                response.IsSuccessStatusCode,
                dateTimeProvider.UtcNow);

            await webhooksService.CreateWebhookDeliveryAttemptAsync(request, cancellationToken);

            logger.LogInformation("Webhook delivery attempt for {WebhookId} recorded with status code {StatusCode}.",
                @event.WebhookId, response.StatusCode);
        }
        catch (Exception)
        {
            var request = new WebhookDeliveryAttemptCreationModel(
                @event.WebhookId,
                jsonPayload,
                null,
                false,
                dateTimeProvider.UtcNow);

            await webhooksService.CreateWebhookDeliveryAttemptAsync(request, cancellationToken);

            logger.LogError("Failed to record webhook delivery attempt for {WebhookId}.",
                @event.WebhookId);
        }
    }
}