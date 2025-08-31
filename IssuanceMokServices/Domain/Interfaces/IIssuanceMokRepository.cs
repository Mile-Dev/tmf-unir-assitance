using IssuanceMokServices.Domain.Entities;

namespace IssuanceMokServices.Domain.Interfaces
{
    public interface IIssuanceMokRepository
    {
        Task<UploadEntities> SaveMetadataUploadAsync(UploadEntities metadata);

        Task<UploadEntities> GetByIdAsync(string id);
    }
}
