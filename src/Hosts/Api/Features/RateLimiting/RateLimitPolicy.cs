namespace SmartLedger.Hosts.Api.Features.RateLimiting;

/// <summary>
/// Contains a set of rate limiting policies applied to different types of operations in the application.
/// </summary>
public sealed class RateLimitPolicy
{
    /// <summary>
    /// Restrictions for read operations (HTTP GET requests).
    /// Used to control load when retrieving data.
    /// </summary>
    public static readonly string ReadOperations = "read-operations";

    /// <summary>
    /// Restrictions for write operations (HTTP POST, PUT, PATCH, DELETE requests).
    /// Used to control the frequency of data modifications.
    /// </summary>
    public static readonly string WriteOperations = "write-operations";

    /// <summary>
    /// Restrictions for authentication operations.
    /// Helps prevent brute force attacks and other authentication abuse.
    /// </summary>
    public static readonly string Authentication = "authentication";

    /// <summary>
    /// Restrictions for search operations.
    /// Used to reduce load during complex or frequent search queries.
    /// </summary>
    public static readonly string SearchOperations = "search-operations";

    /// <summary>
    /// Global restrictions for all requests, regardless of operation type.
    /// Serves as a universal safeguard against excessive load.
    /// </summary>
    public static readonly string Global = "global";

    /// <summary>
    /// Restrictions based on the client's IP address.
    /// Used for per-source control of incoming requests.
    /// </summary>
    public static readonly string IpAddress = "ip-address";
}