using MasterRdsServices.Domain.Entities;

namespace MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IGeneralTypesRepository : IRepository<GeneralType>
    {
        Task<GeneralType?> GetIdentificationByIdAsync(int id, int categoryId);
        Task<List<GeneralType>> GetSubTypesByIdAssitanceTypesAsync(int categoryId);
        Task<List<GeneralType>> GetSubTypesByIdCategoriyAsync(int categoryId);

    }
}
