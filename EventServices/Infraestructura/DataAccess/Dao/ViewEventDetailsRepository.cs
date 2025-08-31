using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para acceder a los detalles de eventos desde la vista ViewEventDetail.
    /// Proporciona métodos específicos para consultar eventos por código y por voucher.
    /// </summary>
    public class ViewEventDetailsRepository(MainContext context) : Repository<ViewEventDetail>(context), IViewEventDetailsRepository
    {
        /// <summary>
        /// Obtiene un detalle de evento por su código de evento y opcionalmente por el código de cliente.
        /// </summary>
        /// <param name="value">Código del evento a buscar.</param>
        /// <param name="clientCode">Código del cliente (opcional).</param>
        /// <returns>El detalle del evento encontrado o null si no existe.</returns>
        public async Task<ViewEventDetail?> GetByCode(string value, int? clientCode = null)
        {
            var query = Entities.Where(item => item.CodeEvent == value);
            if (clientCode is not null)
                query = query.Where(item => item.IdClient == clientCode);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene una lista de detalles de eventos por el valor del voucher y opcionalmente por el código de cliente.
        /// </summary>
        /// <param name="value">Valor del voucher a buscar.</param>
        /// <param name="clientCode">Código del cliente (opcional).</param>
        /// <returns>Lista de detalles de eventos que coinciden con los criterios.</returns>
        public async Task<List<ViewEventDetail>> GetByVoucher(string value, int? clientCode = null)
        {
            var query = Entities.Where(item => item.Voucher == value);
            if (clientCode is not null)
                query = query.Where(item => item.IdClient == clientCode);

            return await query.ToListAsync();
        }
    }
}
