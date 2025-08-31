
namespace StorageS3Services.Common.Interfaces
{
    public interface IS3Service
    {
        Task<string> GetPresignedUploadUrlAsync(string Key, string bucketName, string contentType);

        Task<string> GetPresignedDownloadUrlAsync(string fileKey, string bucketName);

        Task<string> SaveFileToS3Async(string bucketName, string fileName, byte[] fileBytes);

        Task<byte[]> GetFileToS3Async(string bucketName, string fileName);

        Task<Stream> GetFileAsStreamAsync(string bucketName, string directory, string fileName);
    }
}
