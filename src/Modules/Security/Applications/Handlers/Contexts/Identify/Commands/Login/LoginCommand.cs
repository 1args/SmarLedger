using SmartLedger.Common.Contracts.Abstractions;
using SmartLedger.Modules.Security.Contracts.Responses.Identify;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Login;

/// <summary>
/// Represents a command for user login.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Password">Password.</param>
public sealed record LoginCommand(
    string Username,
    string Password) : ICommand<LoginResponse>;