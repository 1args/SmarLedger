using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Abstractions;

/// <summary>
/// Interface that represents a contract for email sending strategies.
/// </summary>
public interface IEmailSendingStrategy
{
    /// <summary>
    /// Unique type identifier for this strategy.
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="request">Model containing recipient and message details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendEmailAsync(EmailSendingModel request, CancellationToken cancellationToken);
}
