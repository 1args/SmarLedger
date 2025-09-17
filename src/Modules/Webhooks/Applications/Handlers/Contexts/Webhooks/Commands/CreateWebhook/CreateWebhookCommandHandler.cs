using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;
using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Abstractions;
using SmartLedger.Modules.Webhooks.Applications.AppServices.Contexts.Webhooks.Services.Models;

namespace SmartLedger.Modules.Webhooks.Applications.Handlers.Contexts.Webhooks.Commands.CreateWebhook;

/// <summary>
/// Handles the logic for processing <see cref="CreateWebhookCommand"/>.
/// </summary>
public sealed class CreateWebhookCommandHandler(
    IWebhooksService webhookService,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateWebhookCommand>
{
    /// <inheritdoc />
    public async Task HandleAsync(CreateWebhookCommand command, CancellationToken cancellationToken)
    {
        var request = new WebhookCreationModel(
            command.EventType, 
            command.CallbackUrl,
            dateTimeProvider.UtcNow);

        await webhookService.CreateWebhookAsync(request, cancellationToken);
    }
}