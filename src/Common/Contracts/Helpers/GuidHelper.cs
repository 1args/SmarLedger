namespace SmartLedger.Common.Contracts.Helpers;

/// <summary>
/// Converts a string representation of a GUID to a Guid object.
/// </summary>
public static class GuidHelper
{
    /// <summary>
    /// Converts a string representation of a GUID to a Guid object.
    /// </summary>
    /// <param name="guidString">String representation of the GUID.</param>
    /// <returns>Converted Guid object.</returns>
    public static Guid ConvertFromStringToGuid(string guidString) =>
        Guid.TryParse(guidString, out var guid)
            ? guid 
            : throw new FormatException($"Invalid GUID format: {guidString}");
}