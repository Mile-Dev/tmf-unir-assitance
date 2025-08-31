using EventServices.Domain.Dto.Query;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los métodos para obtener información sobre las garantías de pago asociadas a un proveedor de eventos.
    /// </summary>
    public interface IViewGuaranteesPaymentEventProviderServices
    {
        /// <summary>
        /// Obtiene una lista de garantías de pago asociadas a un proveedor de eventos específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de eventos.</param>
        /// <returns>Una lista de objetos DTO con la información de las garantías de pago.</returns>
        Task<List<ViewGuaranteesPaymentEventProviderGetDto>> GetGuaranteesPaymentByIdEventProviderAsync(int id);
    }
}
