using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using SmartLedger.Common.Contracts.Options;
using SmartLedger.Common.Infrastructures.FileStorage.Abstractions;

namespace SmartLedger.Common.Infrastructures.FileStorage;

/// <inheritdoc/>
public sealed class MinioFileStorage(
   IMinioClient minioClient,
   IOptions<MinioOptions> minioOptions,
   ILogger<MinioFileStorage> logger) : IMinioFileStorage
{
    private readonly MinioOptions _minioOptions = minioOptions.Value;

    /// <inheritdoc/>
    public async Task UploadFileAsync(string bucketName, string fileName, string format, Stream payload, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading file {FileName} to bucket {BucketName}", fileName, bucketName);

        try
        {
            await EnsureBucketExistsAsync(bucketName, cancellationToken);

            if (payload.CanSeek)
            {
                payload.Seek(0, SeekOrigin.Begin);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(payload)
                .WithObjectSize(payload.Length)
                .WithContentType(format);

            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            logger.LogInformation("File {FileName} uploaded successfully to bucket {BucketName}", fileName, bucketName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while uploading file {FileName} to bucket {BucketName}", fileName, bucketName);
            throw new InvalidOperationException($"Failed to upload file '{fileName}' to bucket '{bucketName}'.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Stream> DownloadFileAsync(string bucketName, string fileName, CancellationToken cancellationToken)
    {
        logger.LogInformation("Downloading file {FileName} from bucket {BucketName}", fileName, bucketName);

        try
        {
            await EnsureBucketExistsAsync(bucketName, cancellationToken);

            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
            memoryStream.Position = 0;

            logger.LogInformation("File {FileName} downloaded successfully from bucket {BucketName}", fileName, bucketName);

            return memoryStream;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while downloading file {FileName} from bucket {BucketName}", fileName, bucketName);
            throw new InvalidOperationException($"Failed to download file '{fileName}' from bucket '{bucketName}'.", ex);
        }
    }

    /// <summary>
    /// Ensures that the specified bucket exists. If it does not exist, it creates the bucket.
    /// </summary>
    private async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExists = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!bucketExists)
            {
                logger.LogInformation("Bucket {BucketName} does not exist. Creating it", bucketName);

                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);

                logger.LogInformation("Bucket {BucketName} created successfully", bucketName);
            }
            else
            {
                logger.LogInformation("Bucket {BucketName} already exists", bucketName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while ensuring bucket {BucketName} exists", bucketName);
            throw new InvalidOperationException($"Failed to ensure bucket '{bucketName}' exists.", ex);
        }
    }
}