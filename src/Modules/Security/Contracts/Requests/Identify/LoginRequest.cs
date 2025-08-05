namespace SmartLedger.Modules.Security.Contracts.Requests.Identify;

/// <summary>
/// Represents a request to log in a user.
/// </summary>
/// <param name="Username">Username.</param>
/// <param name="Password">Password.</param>
public sealed record LoginRequest(
    string Username,
    string Password);