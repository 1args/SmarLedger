using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Factories.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services;

/// <summary>
/// Service for sending emails using configured strategies.
/// </summary>
public sealed class EmailSendingService(
    IEmailSendingStrategiesFactory emailSendingStrategiesFactory) : IEmailSendingService
{
    // <inheritdoc/>
    public async Task SendEmailAsync(EmailSendingModel request, CancellationToken cancellationToken)
    {
        var strategy = emailSendingStrategiesFactory.GetStrategy("smtp");
        await strategy.SendEmailAsync(request, cancellationToken);
    }
}