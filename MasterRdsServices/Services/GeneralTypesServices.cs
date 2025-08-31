using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using System.Linq.Expressions;

namespace MasterRdsServices.Services
{
    public class GeneralTypesServices(ILogger<GeneralTypesServices> logger, IMapper mapper, IGeneralTypesRepository generaltypesdao) : IGeneralTypesServices
    {
        private readonly ILogger<GeneralTypesServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IGeneralTypesRepository _generaltypesdao = generaltypesdao;

        public async Task<List<GeneralTypesQueryDto>> GetGeneralSubType(int categoryId)
        {
            try
            {
                var assistanceSubTypes = await _generaltypesdao.GetSubTypesByIdCategoriyAsync(categoryId);
                var getassistanceSubTypes = _mapper.Map<List<GeneralTypesQueryDto>>(assistanceSubTypes);
                return getassistanceSubTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the General Type: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<List<GeneralTypesQueryDto>> GetAssitanceSubType(int categoryId)
        {
            try
            {
                var assistanceSubTypes = await _generaltypesdao.GetSubTypesByIdAssitanceTypesAsync(categoryId);
                var getassistanceSubTypes = _mapper.Map<List<GeneralTypesQueryDto>>(assistanceSubTypes);
                return getassistanceSubTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the General Type: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<GeneralTypesQueryDto> GetGeneralTypeByIdAsync(int id, int categoryId)
        {
            try
            {
                var generalType = await _generaltypesdao.GetIdentificationByIdAsync(id, categoryId);
                var objectgeneralType = _mapper.Map<GeneralTypesQueryDto>(generalType);
                return objectgeneralType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the General Type: {Message}", ex.Message);
                throw;
            }
        }
    }
}
