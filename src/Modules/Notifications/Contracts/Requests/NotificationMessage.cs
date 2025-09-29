using SmartLedger.Modules.Notifications.Contracts.Enums;

namespace SmartLedger.Modules.Notifications.Contracts.Requests;

/// <summary>
/// Represents a request for sending a notification message.
/// </summary>
/// <param name="Type">Type of notification.</param>
/// <param name="UserId">User ID.</param>
/// <param name="Data">Optional placeholder data for the template.</param>
public sealed record NotificationMessage(
    NotificationType Type,
    Guid UserId,
    Dictionary<string, string>? Data = null);