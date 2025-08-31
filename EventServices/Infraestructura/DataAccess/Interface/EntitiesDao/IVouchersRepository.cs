namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    using EventServices.Domain.Entities;

    /// <summary>
    /// Define la interfaz para el repositorio de Vouchers, proporcionando métodos para operaciones específicas sobre la entidad Voucher.
    /// Hereda de <see cref="IRepository{Voucher}"/> para operaciones CRUD genéricas.
    /// </summary>
    public interface IVouchersRepository : IRepository<Voucher>
    {
        /// <summary>
        /// Obtiene un voucher por su nombre y el identificador del cliente asociado.
        /// </summary>
        /// <param name="nameVoucher">Nombre del voucher a buscar.</param>
        /// <param name="clientId">Identificador del cliente asociado al voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene el voucher encontrado o null si no existe.
        /// </returns>
        Task<Voucher?> GetByNameAsync(string nameVoucher, int clientId);

        /// <summary>
        /// Obtiene el nombre del cliente asociado a un voucher específico.
        /// </summary>
        /// <param name="id">Identificador del voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene el nombre del cliente asociado al voucher.
        /// </returns>
        Task<string> GetClientByVoucherAsync(int id);

        /// <summary>
        /// Verifica si existe un voucher con el nombre y el identificador de cliente especificados.
        /// </summary>
        /// <param name="nameVoucher">Nombre del voucher a buscar.</param>
        /// <param name="clientId">Identificador del cliente asociado al voucher.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado es true si existe el voucher, false en caso contrario.
        /// </returns>
        Task<bool> ExistAsync(string nameVoucher, int clientId);
    }
}
