namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Identify.Models;

/// <summary>
/// Represents a model for user login containing username and password.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Password">Password.</param>
public sealed record LoginModel(
    string Username,
    string Password);