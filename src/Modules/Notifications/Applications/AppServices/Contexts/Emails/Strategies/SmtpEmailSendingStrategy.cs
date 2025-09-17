using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Abstractions;
using SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;
using SmartLedger.Modules.Notifications.Contracts.Options;

namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies;

/// <summary>
/// Service that represents a contract for email sending strategies.
/// </summary>
public sealed class SmtpEmailSendingStrategy(
    IOptions<EmailSendingOptions> options,
    ILogger<SmtpEmailSendingStrategy> logger) : IEmailSendingStrategy
{
    private readonly EmailSendingOptions _options = options.Value;

    /// <inheritdoc/>
    public string Type => "smtp";

    /// <inheritdoc/>
    public async Task SendEmailAsync(EmailSendingModel request, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_options.Username, _options.Email));
        message.To.Add(new MailboxAddress(request.Username, request.Email));
        message.Subject = request.Subject;
        message.Body = new TextPart("html")
        {
            Text = request.Message
        };

        try
        {
            using var client = new SmtpClient();

            await client.ConnectAsync(
                _options.Server, 
                _options.Port,
                _options.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls,
                cancellationToken);

            await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Email}", request.Email);
        }
    }
}