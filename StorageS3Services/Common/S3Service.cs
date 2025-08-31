using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using StorageS3Services.Common.Interfaces;

public class S3Service(IAmazonS3 s3Client, IConfiguration configuration) : IS3Service
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly int _urlExpirationMinutes = int.Parse(configuration["AWS:S3:UrlExpirationMinutes"]);

    public async Task<string> GetPresignedUploadUrlAsync(string Key, string bucketName, string contentType)
    {


        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = Key,
            Expires = DateTime.UtcNow.AddMinutes(_urlExpirationMinutes),
            Verb = HttpVerb.PUT,
            ContentType = contentType
        };

        return await _s3Client.GetPreSignedURLAsync(request);

    }

    public async Task<string> GetPresignedDownloadUrlAsync(string fileKey, string bucketName)
    {
        try
        {
            var metadataRequest = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            await _s3Client.GetObjectMetadataAsync(metadataRequest);


            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(_urlExpirationMinutes),
                Verb = HttpVerb.GET
            };

            return await _s3Client.GetPreSignedURLAsync(request);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new string($"The requested file does not exist in the system. Please check the file name and try again.");
        }
    }

    public async Task<string> SaveFileToS3Async(string bucketName, string fileName, byte[] fileBytes)
    {
        using var stream = new MemoryStream(fileBytes);
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = fileName,
            BucketName = bucketName,
            ContentType = "application/pdf"
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);
        return $"s3://{bucketName}/{fileName}";
    }

    public async Task<byte[]> GetFileToS3Async(string bucketName, string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        using var response = await _s3Client.GetObjectAsync(request);
        using var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);

        var fileBytes = memoryStream.ToArray();
        return fileBytes;
    }

    public async Task<Stream> GetFileAsStreamAsync(string bucketName, string directory, string fileName)
    {

        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = $"{directory}/{fileName}"
        };

        var response = await _s3Client.GetObjectAsync(request);
        return response.ResponseStream;
    }

}

