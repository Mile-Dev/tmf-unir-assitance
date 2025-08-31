using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace MasterRdsServices.Services
{
    public class EventProviderStatusServices(ILogger<EventProviderStatusServices> logger, IMapper mapper, IEventProviderStatusRepository eventproviderstatusDao) : IEventProviderStatusServices
    {

        private readonly ILogger<EventProviderStatusServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IEventProviderStatusRepository _eventproviderstatusDao = eventproviderstatusDao;

        public List<EventProviderStatusQueryDto> GetEventProviderStatus()
        {
            try
            {
                var eventproviderstatus = _eventproviderstatusDao.GetAll();
                var getAlleventproviderstatus = _mapper.Map<List<EventProviderStatusQueryDto>>(eventproviderstatus);
                return getAlleventproviderstatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the eventproviderstatus: {Message}", ex.Message);
                throw;
            }
        }
    }
}
