using SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider.Abstractions;

namespace SmartLedger.Common.Applications.AppServices.Services.DateTimeProvider;

/// <inheritdoc />
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}