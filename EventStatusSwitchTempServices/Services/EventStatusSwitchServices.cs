using AutoMapper;
using EventStatusSwitchTempServices.Domain.Dto;
using EventStatusSwitchTempServices.Infraestructura.Interface.EntitiesDao;

namespace EventStatusSwitchTempServices.Services
{
    public class EventStatusSwitchServices(IEventsRepository eventsRepository, IEventStatusRepository eventStatusRepository, IMapper mapper,
                                            ILogger<EventStatusSwitchServices> logger) : IEventStatusSwitchServices
    {
        public readonly IEventsRepository _eventsRepository = eventsRepository;

        public readonly IEventStatusRepository _eventStatusRepository = eventStatusRepository;

        public readonly IMapper _mapper = mapper;

        public readonly ILogger<EventStatusSwitchServices> _logger = logger;

        public async Task<ResponseEventStatusSwitch> UpdateStatusEventById(RequestEventStatus eventSwitch)
        {
            if (eventSwitch == null)
                throw new ArgumentNullException(nameof(eventSwitch), "The request cannot be null.");

            var responseEventStatus = await _eventStatusRepository.GetByIdAsync(eventSwitch.IdStatusEvent);
            if (responseEventStatus == null)
                return new ResponseEventStatusSwitch
                {
                    Success = false,
                    ErrorMessage = "Event Status not found"
                };

            var responseEvent = await _eventsRepository.GetByIdAsync(eventSwitch.Id);
            if (responseEvent == null)
                return new ResponseEventStatusSwitch
                {
                    Success = false,
                    ErrorMessage = "Event not found"
                };

            // Actualizar entidad
            responseEvent.EventStatusId = eventSwitch.IdStatusEvent;
            responseEvent.UpdatedAt = DateTime.UtcNow;

            // Intentar actualizar
            var updateSuccess = _eventsRepository.Update(responseEvent, out var errorMessage);

            if (!updateSuccess)
                return new ResponseEventStatusSwitch
                {
                    Success = false,
                    ErrorMessage = errorMessage
                };

            return new ResponseEventStatusSwitch
            {
                Success = updateSuccess,
                Id = responseEvent.Id,
                UpdatedAt = responseEvent.UpdatedAt,
                EventStatusId = eventSwitch.IdStatusEvent
            };
        }
    }
}