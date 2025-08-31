using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la gestión de la entidad <see cref="ContactInformation"/>.
    /// Proporciona métodos para consultar información de contacto asociada a viajes de clientes.
    /// </summary>
    public class ContactInformationRepository : Repository<ContactInformation>, IContactInformationRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ContactInformationRepository"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos principal.</param>
        public ContactInformationRepository(MainContext context) : base(context) { }

        /// <summary>
        /// Verifica si existen valores de información de contacto para un viaje de cliente y una lista de tipos generales.
        /// </summary>
        /// <param name="customerTripId">Identificador del viaje del cliente.</param>
        /// <param name="generalTypesIds">Lista de identificadores de tipos generales.</param>
        /// <returns>Lista de valores encontrados.</returns>
        public async Task<List<string>> ExistValue(int customerTripId, List<int> generalTypesIds)
        {
            return await Entities
                         .Where(item => item.CustomerTripId == customerTripId && generalTypesIds.Contains(item.GeneralTypesId))
                         .Select(item => item.Value)
                         .ToListAsync();
        }

        /// <summary>
        /// Obtiene la información de contacto por su valor y tipo general.
        /// </summary>
        /// <param name="value">Valor de la información de contacto.</param>
        /// <param name="idGeneralTypes">Identificador del tipo general.</param>
        /// <returns>Entidad <see cref="ContactInformation"/> encontrada o null si no existe.</returns>
        public async Task<ContactInformation?> GetByValue(string value, int idGeneralTypes)
            => await Entities
                   .Include(item => item.CustomerTrip)
                   .FirstOrDefaultAsync(item => item.Value == value && item.GeneralTypesId == idGeneralTypes);

    }
}
