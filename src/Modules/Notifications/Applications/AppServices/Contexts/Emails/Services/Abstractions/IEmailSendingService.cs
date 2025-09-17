using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services.Abstractions;

/// <summary>
/// Interface for sending emails using configured strategies.
/// </summary>
public interface IEmailSendingService
{
    /// <summary>
    /// Sends an email message.
    /// </summary>
    /// <param name="request">Model containing recipient and message details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendEmailAsync(EmailSendingModel request, CancellationToken cancellationToken);
}
