using SmartLedger.Modules.Webhooks.Applications.AppServices.Models;
using SmartLedger.Modules.Webhooks.Infrastructures.DataAccess.Contexts.Models;

namespace SmartLedger.Modules.Webhooks.Applications.AppServices.Abstractions;

/// <summary>
/// Provides functionality for receiving transactions.
/// </summary>
public interface IWebhooksService
{
    /// <summary>
    /// Creates a new webhook.
    /// </summary>
    /// <param name="request">Model containing webhook creation data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CreateWebhookAsync(WebhookCreationModel request, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a list of webhooks for a specific event type.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of webhooks.</returns>
    Task<List<Webhook>> GetWebhooksAsync(string eventType, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new webhook delivery attempt record.
    /// </summary>
    /// <param name="request">Model containing webhook delivery attempt creation data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CreateWebhookDeliveryAttemptAsync(WebhookDeliveryAttemptCreationModel request, CancellationToken cancellationToken);
}