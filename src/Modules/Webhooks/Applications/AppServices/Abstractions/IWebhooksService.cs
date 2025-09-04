using SmartLedger.Modules.Webhooks.Applications.AppServices.Models;

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
}