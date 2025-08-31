using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IPhoneConsultationRepository : IRepository<PhoneConsultation>
    {
        Task<List<PhoneConsultation>> ListPhoneConsultationAsync(int eventId);

        Task<bool> ExistsRecordScheduledAsync(int eventId);

    }
}
