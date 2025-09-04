using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Models;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Applications.AppServices;

/// <inheritdoc />
public sealed class WebhooksService(
    IRepository<Webhook, WebhooksDbContext> webhooksRepository,
    ILogger<WebhooksService> logger) : IWebhooksService
{
    /// <inheritdoc />
    public async Task CreateWebhookAsync(WebhookCreationModel request, CancellationToken cancellationToken)
    {
        var webhookSubscription = new Webhook
        {
            EventType = request.EventType,
            CallbackUrl = request.CallbackUrl,
            CreatedAt = request.CreatedAt
        };

        await webhooksRepository.AddAsync(webhookSubscription, cancellationToken);
    }
}