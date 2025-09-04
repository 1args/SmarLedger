using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLedger.Common.Infrastructures.DataAccess.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Abstractions;
using SmartLedger.Modules.Webhooks.Contracts.Responses;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Applications.AppServices;

/// <inheritdoc />
public sealed class WebhooksDispatcher(
    IHttpClientFactory httpClientFactory,
    IRepository<Webhook, WebhooksDbContext> webhooksRepository,
    ILogger<WebhooksDispatcher> logger) : IWebhooksDispatcher
{
    /// <inheritdoc />
    public async Task DispatchAsync<TData>(string eventType, TData payload, CancellationToken cancellationToken)
    {
        var webhooks = await webhooksRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(w => w.EventType == eventType)
            .ToListAsync(cancellationToken);

        foreach (var webhook in webhooks)
        {
            using var httpClient = httpClientFactory.CreateClient();

            var request = new WebhookResponse<TData>(
                Guid.NewGuid(),
                webhook.EventType,
                webhook.Id, 
                DateTime.UtcNow, 
                payload);

            await httpClient.PostAsJsonAsync(webhook.CallbackUrl, request, cancellationToken);
        }
    }
}