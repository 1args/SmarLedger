namespace SmartLedger.Modules.Security.Contracts.Requests.Identify;

/// <summary>
/// Represents a request to reset a user's password.
/// </summary>
/// <param name="CurrentPassword">Current password.</param>
/// <param name="NewPassword">New password.</param>
public sealed record ResetPasswordRequest(
    string CurrentPassword,
    string NewPassword);