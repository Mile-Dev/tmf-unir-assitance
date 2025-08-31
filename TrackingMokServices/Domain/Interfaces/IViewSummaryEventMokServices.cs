using SharedServices.Objects;
using TrackingMokServices.Domain.Dto;

namespace TrackingMokServices.Domain.Interfaces
{
    public interface IViewSummaryEventMokServices
    {
        Task<List<ResponseEventMok>> ListEventMokAsync(string id);

        Task<ResponseEventMok> UpdateTracking(string id);

        Task<ResponseEventMok> CreatedTracking(RequestMok input);

    }
}
