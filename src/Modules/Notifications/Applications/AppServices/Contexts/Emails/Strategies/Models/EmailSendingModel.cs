namespace SmartLedger.Modules.Notifications.Applications.AppServices.Contexts.Emails.Strategies.Models;

/// <summary>
/// Represents the data required to send an email message.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Email">Email address.</param>
/// <param name="Subject">Subject of the email.</param>
/// <param name="Message">Body content of the email.</param>
public sealed record EmailSendingModel(
    string Username,
    string Email,
    string Subject,
    string Message);