namespace SmartLedger.Modules.Security.Clients.Keycloak.Converters;

/// <summary>
/// Converts Unix timestamps in milliseconds and seconds to UTC DateTime.
/// </summary>
public static class DateTimeConverter
{
    /// <summary>Unix epoch start date (January 1, 1970, 00:00:00 UTC).</summary>
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Converts a Unix timestamp in milliseconds to UTC DateTime.
    /// </summary>
    /// <param name="unixMillis">Timestamp in milliseconds.</param>
    /// <returns>Converted Timestamp to DateTime.</returns>
    public static DateTime FromUnixMillisToDateTimeUtc(long unixMillis) => 
        UnixEpoch.AddMilliseconds(unixMillis);

    /// <summary>
    /// Converts a Unix timestamp in seconds to UTC DateTime.
    /// </summary>
    /// <param name="unixSeconds">Timestamp in seconds.</param>
    /// <returns>Converted Timestamp to DateTime.</returns>
    public static DateTime FromUnixSecondsToDateTimeUtc(long unixSeconds) =>
        UnixEpoch.AddSeconds(unixSeconds);
}