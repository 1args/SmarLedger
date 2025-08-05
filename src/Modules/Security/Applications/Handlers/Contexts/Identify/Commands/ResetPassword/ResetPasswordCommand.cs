using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.ResetPassword;

/// <summary>
/// Represents a command to reset a user's password.
/// </summary>
/// <param name="CurrentPassword">Current password.</param>
/// <param name="NewPassword">New password.</param>
public sealed record ResetPasswordCommand(
    string CurrentPassword,
    string NewPassword) : ICommand;