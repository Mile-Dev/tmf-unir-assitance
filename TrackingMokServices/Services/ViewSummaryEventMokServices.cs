using AutoMapper;
using SharedServices.Objects;
using System.Globalization;
using System.Threading;
using TrackingMokServices.Domain.Dto;
using TrackingMokServices.Domain.Interfaces;
using TrackingMokServices.Infraestructura.DataAccess.Interface;

namespace TrackingMokServices.Services
{
    public class ViewSummaryEventMokServices(IUnitOfWork unitOfWork, ILogger<ViewSummaryEventMokServices> logger, IMapper mapper) : IViewSummaryEventMokServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ViewSummaryEventMokServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public Task<ResponseEventMok> CreatedTracking(RequestMok input)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResponseEventMok>> ListEventMokAsync(string id)
        {
            try
            {
                var vieweventsmok = await _unitOfWork.ViewSummaryEventMokRepository.ListEventMokAsync(id);
                var eventmoklist =  _mapper.Map<List<ResponseEventMok>>(vieweventsmok);
                return eventmoklist;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while geting the ListEventMokAsync: {Message}", ex.Message);
                throw;
            }
        }

        public Task<ResponseEventMok> UpdateTracking(string id)
        {
            throw new NotImplementedException();
        }
    }
}
