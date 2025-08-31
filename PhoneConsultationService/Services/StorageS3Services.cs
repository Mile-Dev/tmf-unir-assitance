using PhoneConsultationService.Common.Constans;
using PhoneConsultationService.Domain.Dto.Create;
using PhoneConsultationService.Domain.Dto.Query;
using PhoneConsultationService.Domain.Interfaces;
using StorageS3Services.Common.Interfaces;

namespace PhoneConsultationService.Services;

public class StorageS3Services(IS3Service s3Service, IConfiguration configuration, ILogger<StorageS3Services> logger): IStorageS3Services
{
    public IS3Service _s3Service = s3Service;
    private readonly string _bucketName = configuration["AWS:S3:BucketName"]
               ?? throw new ArgumentNullException("BucketName is not configured");
    private readonly ILogger<StorageS3Services> _logger = logger;

    public async Task<string> GetPresignedUploadUrlAsync(int eventNumber, DocumentUploadDto documentUploadDto)
    {
        string Key = $"{documentUploadDto.Path}/{documentUploadDto.FileName}";
        _logger.LogInformation("Generando URL prefirmada para subir el archivo: {Key} con tipo de contenido: {ContentType} en el bucket: {BucketName}", Key, documentUploadDto.ContentType, _bucketName);
        var contentTypeDecode = Uri.UnescapeDataString(documentUploadDto.ContentType);
        string presignedUrl = await _s3Service.GetPresignedUploadUrlAsync(Key, _bucketName, contentTypeDecode);
        _logger.LogInformation("La URL prefirmada para subir el archivo es: {PresignedUrl}", presignedUrl);
        return presignedUrl;
    }

    public async Task<string> GetPresignedDownloadUrlAsync(int eventNumber, DocumentDownloadDto documentDownloadDto)
    {
        var fileKey = $"{documentDownloadDto.Path}/{documentDownloadDto.FileName}";
        string presignedUrl = await _s3Service.GetPresignedDownloadUrlAsync(fileKey, _bucketName);
        _logger.LogInformation("Descarga el archivo usando esta URL: {PresignedUrl}", presignedUrl);
        return presignedUrl;
    }
}
