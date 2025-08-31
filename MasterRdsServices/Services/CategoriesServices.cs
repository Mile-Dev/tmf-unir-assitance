using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using System.Linq.Expressions;

namespace MasterRdsServices.Services
{
    public class CategoriesServices(ILogger<CategoriesServices> logger, IMapper mapper, ICategoriesRepository categoriesDao) : ICategoriesServices
    {

        private readonly ILogger<CategoriesServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ICategoriesRepository _categoriesDao = categoriesDao;

        public List<CategoriesQueryDto> GetAllCategories()
        {
            try
            {
                var categories = _categoriesDao.GetAll();
                var getAllCategories = _mapper.Map<List<CategoriesQueryDto>>(categories);
                return getAllCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the category: {Message}", ex.Message);
                throw;
            }
        }

        public List<CategoriesQueryDto> GetCategoriesAssits()
        {
            try
            {
                var filters = new List<Expression<Func<Category, bool>>>
                                    {
                                        x => x.IsConfigurationField != true
                                    };
                var categories = _categoriesDao.GetAll(filters);
                var getAllCategories = _mapper.Map<List<CategoriesQueryDto>>(categories);
                return getAllCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the category: {Message}", ex.Message);
                throw;
            }
        }
    }
}
