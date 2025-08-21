namespace SmartLedger.Common.Infrastructures.FileStorage.Abstractions;

/// <summary>
/// Handles uploading files to MinIO object storage.
/// </summary>
public interface IMinioFileStorage
{
    /// <summary>
    /// Uploads a file to the specified bucket.
    /// </summary>
    /// <param name="bucketName">Name of the target bucket where the file will be stored.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="format">Format of the file.</param>
    /// <param name="payload">Stream containing the file's data to upload.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UploadFileAsync(string bucketName, string fileName, string format, Stream payload, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads a file from the specified bucket.
    /// </summary>
    /// <param name="bucketName">Name of the target bucket where the file will be stored.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing the downloaded file.</returns>
    Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken);
}