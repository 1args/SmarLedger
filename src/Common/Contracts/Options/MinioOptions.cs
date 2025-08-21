namespace SmartLedger.Common.Contracts.Options;

/// <summary>
/// Options for configuring Minio object storage.
/// </summary>
public sealed class MinioOptions
{
    /// <summary>Minio server endpoint.</summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>Access key for Minio.</summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>Secret key for Minio.</summary>
    public string SecretKey { get; set; } = string.Empty;
}