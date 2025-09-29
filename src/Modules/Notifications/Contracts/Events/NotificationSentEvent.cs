using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Notifications.Contracts.Enums;

namespace SmartLedger.Modules.Notifications.Contracts.Events;

/// <summary>
/// Event triggered when a notification has been sent.
/// </summary>
/// <param name="Type">Type of notification.</param>
/// <param name="UserId">User ID.</param>
/// <param name="Data">Optional placeholder data for templates.</param>
public sealed record NotificationSentEvent(
    NotificationType Type,
    Guid UserId,
    Dictionary<string, string>? Data = null) : Event;