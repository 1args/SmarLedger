using SmartLedger.Common.Host.Consumers;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Events;
using SmartLedger.Modules.Notifications.Contracts.Requests;

namespace SmartLedger.Modules.Notifications.Host.Consumers.Consumers;

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