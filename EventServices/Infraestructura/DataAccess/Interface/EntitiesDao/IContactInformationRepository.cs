using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IContactInformationRepository : IRepository<ContactInformation>
    {
        Task<ContactInformation?> GetByValue(string value, int idGeneralTypes);

        Task<List<string>> ExistValue(int customerTripId, List<int> generalTypesIds);

    }
}
