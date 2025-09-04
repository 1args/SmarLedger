namespace SmartLedger.Modules.Webhooks.Contracts.Common;

/// <summary>
/// Validation constants.
/// </summary>
public static class ValidationConstants
{
    /// <summary>Minimum length for a password.</summary>
    public const int EventTypeMinLength = 8;

    /// <summary>Maximum length for a password.</summary>
    public const int EventTypeMaxLength = 64;

    /// <summary>Maximum length for a callback url.</summary>
    public const int CallbackUrlMaxLength = 200;
}