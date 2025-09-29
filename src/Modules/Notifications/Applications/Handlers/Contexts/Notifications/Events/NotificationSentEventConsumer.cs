using SmartLedger.Common.Applications.Handlers.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Events;
using SmartLedger.Modules.Notifications.Contracts.Requests;

namespace SmartLedger.Modules.Notifications.Applications.Handlers.Contexts.Notifications.Events;

/// <summary>
/// Consumes the <see cref="NotificationSentEvent"/>.
/// </summary>
public sealed class NotificationSentEventConsumer(
    INotificationService notificationService) : IEventConsumer<NotificationSentEvent>
{
    /// <inheritdoc />
    public async Task ConsumeAsync(NotificationSentEvent @event, CancellationToken cancellationToken)
    {
        var request = new NotificationMessage(
            @event.Type,
            @event.UserId,
            @event.Data);

        await notificationService.SendEmailAsync(request, cancellationToken);
    }
}