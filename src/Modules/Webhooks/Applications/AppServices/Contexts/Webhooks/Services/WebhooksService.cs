using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Models;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services;

/// <inheritdoc />
public sealed class WebhooksService(
    IRepository<Webhook, WebhooksDbContext> webhooksRepository,
    IRepository<WebhookDeliveryAttempt, WebhooksDbContext> webhooksDeliveryAttemptsRepository,
    ILogger<WebhooksService> logger) : IWebhooksService
{
    /// <inheritdoc />
    public async Task CreateWebhookAsync(WebhookCreationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating a new webhook for event type {EventType} with callback URL {CallbackUrl}",
            request.EventType, request.CallbackUrl);

        var webhook = new Webhook
        {
            EventType = request.EventType,
            CallbackUrl = request.CallbackUrl,
            CreatedAt = request.CreatedAt
        };

        await webhooksRepository.AddAsync(webhook, cancellationToken);

        logger.LogInformation(
            "Webhook for event type {EventType} with callback URL {CallbackUrl} and ID {WebhookId} created successfully",
            request.EventType, request.CallbackUrl, webhook.Id);
    }

    /// <inheritdoc />
    public async Task<List<Webhook>> GetWebhooksAsync(string eventType, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving webhooks for event type {EventType}", eventType);

        var webhooks = await webhooksRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(w => w.EventType == eventType)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Retrieved {WebhookCount} webhooks for event type {EventType}",
            webhooks.Count, eventType);

        return webhooks;
    }

    /// <inheritdoc />
    public async Task CreateWebhookDeliveryAttemptAsync(WebhookDeliveryAttemptCreationModel request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating a new webhook delivery attempt for webhook ID {WebhookId} with status code {StatusCode}",
            request.WebhookId, request.ResponseStatusCode);

        var webhookDeliveryAttempt = new WebhookDeliveryAttempt
        {
            WebhookId = request.WebhookId,
            Payload = request.Payload,
            ResponseStatusCode = request.ResponseStatusCode,
            IsSuccess = request.IsSuccess,
            AttemptedAt = request.AttemptedAt,
        };

        await webhooksDeliveryAttemptsRepository.AddAsync(webhookDeliveryAttempt, cancellationToken);

        logger.LogInformation(
            "Webhook delivery attempt for webhook ID {WebhookId} with status code {StatusCode} created successfully",
            request.WebhookId, request.ResponseStatusCode);
    }
}