using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace MasterRdsServices.Services
{
    public class EventStatusServices(ILogger<EventStatusServices> logger, IMapper mapper, IEventStatusRepository eventstatusDao) : IEventStatusServices
    {

        private readonly ILogger<EventStatusServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IEventStatusRepository _eventstatusDao = eventstatusDao;
        public List<EventStatusQueryDto> GetEventStatusAsync()
        {
            try
            {
                var eventstatus = _eventstatusDao.GetAll();
                var getAlleventstatus = _mapper.Map<List<EventStatusQueryDto>>(eventstatus);
                return getAlleventstatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the eventproviderstatus: {Message}", ex.Message);
                throw;
            }
        }
    }
}
