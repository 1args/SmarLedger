using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Logout;

/// <summary>
/// Represents a command to log out a user.
/// </summary>
public sealed record LogoutCommand : ICommand;