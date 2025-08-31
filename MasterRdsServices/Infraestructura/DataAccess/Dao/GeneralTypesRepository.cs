using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Infraestructura.DataAccess.Common;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using MasterRdsServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace MasterRdsServices.Infraestructura.DataAccess.Dao
{
    public class GeneralTypesRepository(MainContext context) : Repository<GeneralType>(context), IGeneralTypesRepository
    {
        public async Task<GeneralType?> GetIdentificationByIdAsync(int id, int categoryId)
        {
            return await Entities
                .Include(gt => gt.Category)
                .FirstOrDefaultAsync(gt => gt.Id == id && gt.CategoriesId == categoryId);
        }

        public async Task<List<GeneralType>> GetSubTypesByIdCategoriyAsync(int categoryId)
        {
            return await Entities
                         .Include(gt => gt.Category)
                         .Where(gt => gt.CategoriesId == categoryId && gt.Category != null && gt.Category.IsConfigurationField == true)
                         .ToListAsync();
        }

        public async Task<List<GeneralType>> GetSubTypesByIdAssitanceTypesAsync(int categoryId)
        {
            return await Entities
                         .Include(gt => gt.Category)
                         .Where(gt => gt.CategoriesId == categoryId && gt.Category != null && gt.Category.IsConfigurationField != true)
                         .ToListAsync();
        }
    }
}
