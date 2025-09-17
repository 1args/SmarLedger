using SmartLedger.Modules.Notifications.Contracts.Enums;

namespace SmartLedger.Modules.Notifications.Contracts.Requests;

/// <summary>
/// Represents a request for sending a notification message.
/// </summary>
/// <param name="Type">Type of notification.</param>
/// <param name="Username">User name.</param>
/// <param name="Email">Email address.</param>
/// <param name="Data">Optional placeholder data for the template.</param>
public sealed record NotificationMessage(
    NotificationType Type,
    string Username,
    string Email,
    Dictionary<string, string>? Data = null);