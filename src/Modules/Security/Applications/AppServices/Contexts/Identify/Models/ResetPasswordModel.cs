namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;

/// <summary>
/// Represents a model for resetting a user's password.
/// </summary>
/// <param name="CurrentPassword">Current password.</param>
/// <param name="NewPassword">New password.</param>
public sealed record ResetPasswordModel(
    string CurrentPassword,
    string NewPassword);