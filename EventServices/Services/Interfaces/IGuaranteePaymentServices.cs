using EventServices.Domain.Dto.Create;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los servicios para la gestión de pagos en garantía.
    /// </summary>
    public interface IGuaranteePaymentServices
    {
        /// <summary>
        /// Crea un nuevo pago en garantía.
        /// </summary>
        /// <param name="eventProviderDto">Datos del pago en garantía a crear.</param>
        /// <returns>Un objeto con la información del registro creado.</returns>
        Task<ResponseCreatedDto> CreatedGuaranteePaymentAsync(GuaranteePaymentDto eventProviderDto);

        /// <summary>
        /// Obtiene la información de un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador del pago en garantía.</param>
        /// <returns>Datos del pago en garantía.</returns>
        Task<Domain.Dto.Query.GuaranteePaymentDto> GetGuaranteePaymentAsync(int id);

        /// <summary>
        /// Actualiza la información de un pago en garantía existente.
        /// </summary>
        /// <param name="id">Identificador del pago en garantía.</param>
        /// <param name="guaranteepayment">Datos actualizados del pago en garantía.</param>
        /// <returns>Datos actualizados del pago en garantía.</returns>
        Task<GuaranteePaymentDto> UpdatedGuaranteePaymentAsync(int id, GuaranteePaymentDto guaranteepayment);

        /// <summary>
        /// Elimina un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador del pago en garantía.</param>
        /// <returns>True si la eliminación fue exitosa, de lo contrario false.</returns>
        Task<bool> DeletedGuaranteePaymentByIdAsync(int id);

        /// <summary>
        /// Cancela un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador del pago en garantía.</param>
        /// <returns>True si la cancelación fue exitosa, de lo contrario false.</returns>
        Task<bool> CanceledGuaranteePaymentByIdAsync(int id);
    }
}
