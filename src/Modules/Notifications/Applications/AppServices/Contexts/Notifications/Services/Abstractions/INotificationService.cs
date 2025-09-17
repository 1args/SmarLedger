using SmartLedger.Modules.Notifications.Contracts.Requests;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services.Abstractions;

/// <summary>
/// Interface for sending different types of notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends an email notification.
    /// </summary>
    /// <param name="request">Notification request containing recipient and message details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendEmailAsync(NotificationMessage request, CancellationToken cancellationToken);
}