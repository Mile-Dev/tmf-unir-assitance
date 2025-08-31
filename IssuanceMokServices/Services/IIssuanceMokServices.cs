using IssuanceMokServices.Domain.Dto;

namespace IssuanceMokServices.Services
{
    public interface IIssuanceMokServices
    {
        Task<UploadResponse> GetPresignUrlIssuanceAsync(UploadRequest documentMetadata);

        Task<UploadResponse> SaveDocumentIssuanceAsync( UploadRequest documentMetadata);

        Task<UploadResponse> UpdateDocumentIssuanceAsync(string id, UploadRequest documentMetadata);

        Task<UploadResponseQuery> DownloadUrllssuanceAsync(string id);
    }
}
