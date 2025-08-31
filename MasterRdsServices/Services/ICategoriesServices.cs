using MasterRdsServices.Domain.Dto;

namespace MasterRdsServices.Services
{
    public interface ICategoriesServices
    {

        List<CategoriesQueryDto> GetAllCategories();
        List<CategoriesQueryDto> GetCategoriesAssits();

    }
}
