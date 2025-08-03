namespace SmartLedger.Modules.Security.Clients.Keycloak.Converters;

/// <summary>
/// Converts a string representation of a GUID to a Guid object.
/// </summary>
public static class GuidConverter
{
    /// <summary>
    /// Converts a string representation of a GUID to a Guid object.
    /// </summary>
    /// <param name="guidString">String representation of the GUID.</param>
    /// <returns>Converted Guid object.</returns>
    public static Guid FromStringToGuid(string guidString) =>
        Guid.TryParse(guidString, out var guid)
            ? guid 
            : throw new FormatException($"Invalid GUID format: {guidString}");
}