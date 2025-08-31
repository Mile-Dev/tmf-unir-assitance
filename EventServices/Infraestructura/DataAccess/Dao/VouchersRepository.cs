using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la gestión de entidades <see cref="Voucher"/> en la base de datos.
    /// Proporciona métodos específicos para la obtención de vouchers por nombre y cliente,
    /// así como la obtención del nombre del cliente asociado a un voucher.
    /// </summary>
    public class VouchersRepository(MainContext context) : Repository<Voucher>(context), IVouchersRepository
    {
        /// <summary>
        /// Obtiene un voucher por su nombre y el identificador del cliente asociado.
        /// </summary>
        /// <param name="nameVoucher">Nombre del voucher a buscar.</param>
        /// <param name="clientId">Identificador del cliente asociado al voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene el voucher encontrado o null si no existe.
        /// </returns>
        public Task<Voucher?> GetByNameAsync(string nameVoucher, int clientId)
            => Entities
                .FirstOrDefaultAsync(item => item.Name == nameVoucher && item.ClientId == clientId);

        /// <summary>
        /// Verifica si existe un voucher con el nombre y el identificador de cliente especificados.
        /// </summary>
        /// <param name="nameVoucher">Nombre del voucher a buscar.</param>
        /// <param name="clientId">Identificador del cliente asociado al voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado es true si existe el voucher, false en caso contrario.
        /// </returns>
        public Task<bool> ExistAsync(string nameVoucher, int clientId)
            => Entities
                .AnyAsync(item => item.Name == nameVoucher && item.ClientId == clientId);

        /// <summary>
        /// Obtiene el nombre del cliente asociado a un voucher específico.
        /// </summary>
        /// <param name="id">Identificador del voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene el nombre del cliente o una cadena vacía si no se encuentra.
        /// </returns>
        public async Task<string> GetClientByVoucherAsync(int id)
        {
            return await Entities
                .Where(v => v.Id == id)
                .Select(v => v.Client.Name)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
    }
}
