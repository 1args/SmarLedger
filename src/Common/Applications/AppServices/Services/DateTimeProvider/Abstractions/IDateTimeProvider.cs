namespace SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;

/// <summary>
/// Provider for working with the current date and time.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>Current date and time.</summary>
    DateTime UtcNow { get; }
}