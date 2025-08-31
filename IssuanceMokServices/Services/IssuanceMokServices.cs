using AutoMapper;
using IssuanceMokServices.Common.Constans;
using IssuanceMokServices.Domain.Dto;
using IssuanceMokServices.Domain.Entities;
using IssuanceMokServices.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using SharedServices.HelperBase64;
using SharedServices.JsonHelper;
using StorageS3Services.Common.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace IssuanceMokServices.Services
{
    public class IssuanceMokServices(IS3Service s3Service, IIssuanceMokRepository repository, ILogger<IssuanceMokServices> logger, IMapper mapper, IConfiguration configuration) : IIssuanceMokServices
    {
        private readonly IIssuanceMokRepository _repository = repository;
        private readonly ILogger<IssuanceMokServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IS3Service _s3Service = s3Service;
        private readonly string _bucketName = configuration["AWS:S3:BucketName"]
                    ?? throw new ArgumentNullException("BucketName is not configured in appsettings.json");

        public async Task<UploadResponse> GetPresignUrlIssuanceAsync(UploadRequest documentMetadata)
        {
            if (string.IsNullOrWhiteSpace(documentMetadata.IssuanceName))
            {
                throw new ArgumentException("Issuance name cannot be null or empty.");
            }

            if (documentMetadata.IssuanceName.Length >= 100)
            {
                throw new ArgumentException("The name must not be longer than 100 characters.");
            }

            documentMetadata.IssuanceName = Regex.Replace(EnsurePdfExtension(documentMetadata.IssuanceName.ToUpper()), @"[^a-zA-Z0-9\.\-\ ]", "");
            string key = documentMetadata.IssuanceName;
            string contentTypeDecode = Uri.UnescapeDataString(Constans.contentType);
            string presignedUrl = await _s3Service.GetPresignedUploadUrlAsync(key, _bucketName, contentTypeDecode);
            var normalizedMetadata = JsonConverterHelper.NormalizeJsonElements(documentMetadata.Metadata);
            string metadataJson = JsonSerializer.Serialize(normalizedMetadata);
            return await SaveInformationIssuanceAsync(documentMetadata, presignedUrl, metadataJson);
        }

        public static string EnsurePdfExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (extension != ".PDF")
            {   
                return fileName + ".PDF";
            }
            return fileName;
        }

        public async Task<UploadResponse> SaveDocumentIssuanceAsync(UploadRequest documentMetadata)
        {
            byte[] fileBytes;
            fileBytes = DocumentValidBytes(documentMetadata);
            DocumentSizeValid(fileBytes);
            DcoumentIsPdf(fileBytes);

            var s3Url = await _s3Service.SaveFileToS3Async(_bucketName, documentMetadata.IssuanceName, fileBytes);

            var normalizedMetadata = JsonConverterHelper.NormalizeJsonElements(documentMetadata.Metadata);
            string metadataJson = JsonSerializer.Serialize(normalizedMetadata);

            UploadResponse uploadResponse = await SaveInformationIssuanceAsync(documentMetadata, s3Url, metadataJson);

            return uploadResponse;
        }

        private async Task<UploadResponse> SaveInformationIssuanceAsync(UploadRequest documentMetadata, string s3Url, string metadataJson)
        {
            UploadEntities uploadEntity = _mapper.Map<UploadEntities>(documentMetadata);
            uploadEntity.Metadata = metadataJson;
            uploadEntity.UrlDocument = s3Url;
            uploadEntity.CreatedAt = DateTime.UtcNow.ToString("o");
            uploadEntity.UpdatedAt = DateTime.UtcNow.ToString("o");
            uploadEntity.PartitionKey = documentMetadata.IssuanceName;

            await _repository.SaveMetadataUploadAsync(uploadEntity);

            var uploadResponse = new UploadResponse
            {
                Id = uploadEntity.PartitionKey,
                IssuanceName = documentMetadata.IssuanceName,
                UrlDocument = s3Url,
                CreatedAt = uploadEntity.CreatedAt,
                UpdatedAt = uploadEntity.UpdatedAt
            };
            return uploadResponse;
        }

        public async Task<UploadResponseQuery> DownloadUrllssuanceAsync(string id)
        {
            var uploadEntity = await _repository.GetByIdAsync(id);
            if (uploadEntity == null)
            {
                throw new ArgumentNullException("The requested data does not exist in the system.");
            }
            var urldownload = await _s3Service.GetPresignedDownloadUrlAsync(id, _bucketName);
            UploadResponseQuery uploadReponse = _mapper.Map<UploadResponseQuery>(uploadEntity);
            uploadReponse.UrlDownload = urldownload;

            return uploadReponse;
        }

        public async Task<UploadResponse> UpdateDocumentIssuanceAsync(string id, UploadRequest documentMetadata)
        {
            var uploadEntity = await _repository.GetByIdAsync(id);
            if (uploadEntity == null)
            {
                throw new ArgumentNullException(nameof(uploadEntity), "Upload entity not found.");
            }

            byte[] fileBytes;
            fileBytes = DocumentValidBytes(documentMetadata);
            DocumentSizeValid(fileBytes);
            DcoumentIsPdf(fileBytes);

            var s3Url = await _s3Service.SaveFileToS3Async(_bucketName, documentMetadata.IssuanceName, fileBytes);

            var normalizedMetadata = JsonConverterHelper.NormalizeJsonElements(documentMetadata.Metadata);
            string metadataJson = JsonSerializer.Serialize(normalizedMetadata);

            uploadEntity.Metadata = metadataJson;
            uploadEntity.UrlDocument = s3Url;
            uploadEntity.UpdatedAt = DateTime.UtcNow.ToString("o");
            uploadEntity.PartitionKey = documentMetadata.IssuanceName;

            await _repository.SaveMetadataUploadAsync(uploadEntity);

            var uploadResponse = new UploadResponse
            {
                Id = uploadEntity.PartitionKey,
                IssuanceName = documentMetadata.IssuanceName,
                UrlDocument = s3Url,
                CreatedAt = uploadEntity.CreatedAt,
                UpdatedAt = uploadEntity.UpdatedAt
            };

            return uploadResponse;
        }

        private static void DcoumentIsPdf(byte[] fileBytes)
        {
            if (!IsPdf(fileBytes))
            {
                throw new ArgumentException("The file is not a valid PDF.");
            }
        }

        private static void DocumentSizeValid(byte[] fileBytes)
        {
            if (fileBytes.Length > 1_048_576)
            {
                throw new ArgumentException("The PDF file exceeds the maximum allowed size of 1MB.");
            }
        }

        private static byte[] DocumentValidBytes(UploadRequest documentMetadata)
        {
            byte[] fileBytes;
            try
            {
                fileBytes = Base64Helper.DecodeBase64Pdf(documentMetadata.IssuanceName);
            }
            catch
            {
                throw new ArgumentException("The base64 content is not valid.");
            }

            return fileBytes;
        }              
  
        private static bool IsPdf(byte[] fileBytes)
        {
            if (fileBytes.Length < 4)
                return false;

            return fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 &&  fileBytes[3] == 0x46;   
        }

    }
}
