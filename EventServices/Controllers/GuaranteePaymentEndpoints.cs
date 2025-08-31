using EventServices.Services.Interfaces;

namespace EventServices.Controllers;

/// <summary>
/// Define los endpoints para la gestión de garantía de pagos de eventos.
/// </summary>
public static class GuaranteePaymentEndpoints
{
    /// <summary>
    /// Mapea los endpoints relacionados con las garantías de pagos de eventos.
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>
    public static void MapGuaranteePaymentDtoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Event Guarantee Payment");

        /// <summary>
        /// Obtiene la lista de garantía de pagos asociados a un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="_ListGuaranteePayment">Servicio de consulta de garantía de pagos.</param>
        /// <returns>Lista de garantía de pagos o NotFound si no existen.</returns>
        group.MapGet("/events/providers/{id}/guaranteepayments", GetGuaranteePaymentsbyIdEventprovider);
        static async Task<IResult> GetGuaranteePaymentsbyIdEventprovider(int id, IViewGuaranteesPaymentEventProviderServices _ListGuaranteePayment)
        {
            try
            {
                var result = await _ListGuaranteePayment.GetGuaranteesPaymentByIdEventProviderAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene la información de un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <param name="_GuaranteePayment">Servicio de gestión de garantía de pagos.</param>
        /// <returns>Datos del pago en garantía o NotFound si no existe.</returns>
        group.MapGet("/events/providers/guaranteepayments/{id}", GetGuaranteePaymentsByIdAsync);
        static async Task<IResult> GetGuaranteePaymentsByIdAsync(int id, IGuaranteePaymentServices _GuaranteePayment)
        {
            try
            {
                var result = await _GuaranteePayment.GetGuaranteePaymentAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de una garantía de pago existente.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <param name="input">Datos actualizados de la garantía de pago.</param>
        /// <param name="_GuaranteePayment">Servicio de gestión de garantía de pagos.</param>
        /// <returns>Datos actualizados de la garantía de pago o NotFound si no existe.</returns>
        group.MapPut("/events/providers/guaranteepayments/{id}", UpdateGuaranteePaymentsByIdAsync);
        static async Task<IResult> UpdateGuaranteePaymentsByIdAsync(int id, Domain.Dto.Create.GuaranteePaymentDto input, IGuaranteePaymentServices _GuaranteePayment)
        {
            try
            {
                var result = await _GuaranteePayment.UpdatedGuaranteePaymentAsync(id, input);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo pago en garantía.
        /// </summary>
        /// <param name="input">Datos de la garantía de pago a crear.</param>
        /// <param name="_GuaranteePayment">Servicio de gestión de garantía de pagos.</param>
        /// <returns>Información del registro creado o NotFound si falla.</returns>
        group.MapPost("/events/providers/guaranteepayments", CreatedGuaranteePaymentsAsync);
        static async Task<IResult> CreatedGuaranteePaymentsAsync(Domain.Dto.Create.GuaranteePaymentDto input, IGuaranteePaymentServices _GuaranteePayment)
        {
            try
            {
                var result = await _GuaranteePayment.CreatedGuaranteePaymentAsync(input);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <param name="_GuaranteePayment">Servicio de gestión de garantía de pagos.</param>
        /// <returns>Ok si se eliminó, NotFound si no existe.</returns>
        group.MapDelete("/events/providers/guaranteepayments/{id}", DeleteGuaranteePaymentsAsync);
        static async Task<IResult> DeleteGuaranteePaymentsAsync(int id, IGuaranteePaymentServices _GuaranteePayment)
        {
            try
            {
                var result = await _GuaranteePayment.DeletedGuaranteePaymentByIdAsync(id);
                return result ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela un pago en garantía por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <param name="_GuaranteePayment">Servicio de gestión de garantía de pagos.</param>
        /// <returns>Ok si se canceló, NotFound si no existe.</returns>
        group.MapPatch("/events/providers/guaranteepayments/{id}/cancel", CanceledGuaranteePaymentsAsync);
        static async Task<IResult> CanceledGuaranteePaymentsAsync(int id, IGuaranteePaymentServices _GuaranteePayment)
        {
            try
            {
                var result = await _GuaranteePayment.CanceledGuaranteePaymentByIdAsync(id);
                return result ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
