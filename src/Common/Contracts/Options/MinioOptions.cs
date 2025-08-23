namespace SmartLedger.Common.Contracts.Options;

/// <summary>
/// Options for configuring Minio object storage.
/// </summary>
public sealed class MinioOptions
{
    /// <summary>Minio server endpoint.</summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>Access key.</summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>Secret key.</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Indicates whether to use SSL.</summary>
    public bool UseSsl { get; set; }
}