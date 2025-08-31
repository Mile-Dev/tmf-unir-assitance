using AutoMapper;
using EventFirstContactServices.Domain.Dto.Get;
using EventFirstContactServices.Domain.Entities;
using EventFirstContactServices.Domain.Interfaces;

namespace EventFirstContactServices.Services
{
    public class EventContactEmergencyContactServices(IEventFirstContactRepository<EventEmergencyContact> repositoryEventEmergencyContact,
                                                IMapper mapper,
                                                ILogger<EventContactEmergencyContactServices> logger) : IEventContactEmergencyContactServices
    {

        private readonly IEventFirstContactRepository<EventEmergencyContact> _repositoryEventEmergencyContact = repositoryEventEmergencyContact;
        private readonly ILogger<EventContactEmergencyContactServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<EventFirstContactEmergencyContactGetDto> GetEventFirstContactEmergencyContactGetDtoByIdAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service GetEventFirstContactEmergencyContactGetDtoByIdAsync");
            var contactEmergency = await _repositoryEventEmergencyContact.GetEventDraftByIdAsync(ideventObject, "4");
            return  _mapper.Map<EventFirstContactEmergencyContactGetDto>(contactEmergency);
        }
    }
}
