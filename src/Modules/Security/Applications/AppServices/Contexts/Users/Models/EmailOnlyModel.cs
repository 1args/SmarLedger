namespace SmartLedger.Modules.Security.Applications.AppServices.Contexts.Users.Models;

/// <summary>
/// Model representing a user with only an email address.
/// </summary>
/// <param name="Email"></param>
public sealed record EmailOnlyModel(
    string Email);