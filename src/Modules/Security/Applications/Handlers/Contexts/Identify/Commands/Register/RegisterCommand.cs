using SmartLedger.Common.Contracts.Abstractions;

namespace SmartLedger.Modules.Security.Applications.Handlers.Contexts.Identify.Commands.Register;

/// <summary>
/// Represents a command to register a new user.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Email">Email.</param>
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name.</param>
/// <param name="Password">Password.</param>
public sealed record RegisterCommand(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand;