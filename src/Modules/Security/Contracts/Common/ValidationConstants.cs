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

    /// <summary>Maximum length for a username.</summary>
    public const int UsernameMaxLength = 32;

    /// <summary>Minimum length for a first name.</summary>
    public const int FirstNameMinLength = 2;

    /// <summary>Maximum length for a first name.</summary>
    public const int FirstNameMaxLength = 50;

    /// <summary>Minimum length for a second name.</summary>
    public const int SecondNameMinLength = 2;

    /// <summary>Maximum length for a second name.</summary>
    public const int SecondNameMaxLength = 50;
}