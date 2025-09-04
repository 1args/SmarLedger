using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Webhooks.Applications.Handlers.Commands.CreateWebhookSubscription;

/// <summary>
/// Represents a command to create a new webhook.
/// </summary>
/// <param name="EventType">Event type.</param>
/// <param name="CallbackUrl">Destination URL where the webhook payload will be delivered.</param>
public sealed record CreateWebhookCommand(
    string EventType,
    string CallbackUrl) : ICommand;