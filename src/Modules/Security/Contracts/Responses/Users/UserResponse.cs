namespace SmartLedger.Modules.Security.Contracts.Responses.Users;

/// <summary>
/// Represents a response containing user information from Keycloak.
/// </summary>
/// <param name="Id">Unique identifier of the user.</param>
/// <param name="Username">User name.</param>
/// <param name="FirstName">First name.</param>
/// <param name="LastName">Last name.</param>
/// <param name="Email">Email.</param>
/// <param name="IsEmailVerified">Email confirmation indicator.</param>
/// <param name="CreatedAt">Date and time when user was created.</param>
public sealed record UserResponse(
    Guid Id,
    string Username,
    string FirstName,
    string LastName,
    string Email,
    bool IsEmailVerified,
    DateTime? CreatedAt);