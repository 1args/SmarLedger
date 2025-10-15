using Microsoft.Extensions.Logging;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Services.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Templates.Services.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Requests;
using SmartLedger.Modules.Security.Clients.Keycloak.Abstractions;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Notifications.Services;

/// <summary>
/// Service for sending different types of notifications.
/// </summary>
public sealed class NotificationService(
    IEmailSendingService emailSendingService,
    IKeycloakUserApiClient keycloakUserApiClient,
    ITemplateProvider templateProvider,
    ILogger<NotificationService> logger) : INotificationService
{
    /// <inheritdoc/>
    public async Task SendEmailAsync(NotificationMessage request, CancellationToken cancellationToken)
    {
        var user = await keycloakUserApiClient.GetUserAsync(request.UserId, cancellationToken);

        logger.LogInformation(
            "Sending email notification to {Email} of type {Type}",
            user.Email, request.Type.ToString());

        var updated = request with
        {
            Data = request.Data?
        .Append(new KeyValuePair<string, string>("UserName", user.Username))
        .ToDictionary(kv => kv.Key, kv => kv.Value)
        };

        var (message, subject) = await templateProvider.GetTemplateContentAsync(
            type: request.Type,
            data: updated.Data,
            cancellationToken: cancellationToken);

        if (message is null)
        {
            throw new ArgumentException($"No template found for notification type {request.Type}");
        }
        if (subject is null)
        {
            throw new ArgumentException($"No subject found for notification type {request.Type}");
        }

        var emailRequest = new EmailSendingModel(
            user.Username,
            user.Email,
            subject,
            message);

        await emailSendingService.SendEmailAsync(emailRequest, cancellationToken);

        logger.LogInformation(
            "Email notification sent to {Email} of type {Type}",
            user.Email, request.Type.ToString());
    }
}