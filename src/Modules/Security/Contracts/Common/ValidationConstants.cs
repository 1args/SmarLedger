namespace SmartLedger.Modules.Security.Contracts.Common;

/// <summary>
/// Validation constants.
/// </summary>
public static class ValidationConstants
{
    /// <summary>Minimum length for a password.</summary>
    public const int PasswordMinLength = 8;

    /// <summary>Maximum length for a password.</summary>
    public const int PasswordMaxLength = 64;

    /// <summary>Minimum length for a username.</summary>
    public const int UsernameMinLength = 3;

    /// <summary>TMaximum length for a username.</summary>
    public const int UsernameMaxLength = 32;
}