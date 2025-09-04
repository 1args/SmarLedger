namespace SmartLedger.Modules.Webhooks.Applications.AppServices.Abstractions;

/// <summary>
/// Provides functionality for dispatching webhooks.
/// </summary>
public interface IWebhooksDispatcher
{
    /// <summary>
    /// Dispatches a webhook event with the specified type and payload.
    /// </summary>
    /// <typeparam name="TData">type of the data payload.</typeparam>
    /// <param name="eventType">Event type.</param>
    /// <param name="payload">Data payload associated with the event.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DispatchAsync<TData>(string eventType, TData payload, CancellationToken cancellationToken);
}