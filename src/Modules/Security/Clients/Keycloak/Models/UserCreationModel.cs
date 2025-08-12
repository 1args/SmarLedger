namespace SmartLedger.Modules.Security.Clients.Keycloak.Models;

/// <summary>
/// Represents a model for user creation in Keycloak.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Email">Email.</param>
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name.</param>
/// <param name="Password">Password.</param>
public sealed record UserCreationModel(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password);