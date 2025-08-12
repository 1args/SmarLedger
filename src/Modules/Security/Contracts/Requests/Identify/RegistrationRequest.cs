namespace SmartLedger.Modules.Security.Contracts.Requests.Identify;

/// <summary>
/// Represents the request for user registration.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Email">Email.</param>
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name.</param>
/// <param name="Password">Password.</param>
public sealed record RegistrationRequest(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password);