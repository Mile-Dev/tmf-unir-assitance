using MasterRdsServices.Domain.Dto;

namespace MasterRdsServices.Services
{
    public interface IGeneralTypesServices
    {
        Task<List<GeneralTypesQueryDto>> GetAssitanceSubType(int categoryId);

        Task<GeneralTypesQueryDto> GetGeneralTypeByIdAsync(int id, int categoryId);

        Task<List<GeneralTypesQueryDto>> GetGeneralSubType(int categoryId);

    }
}
