using EventServices.Domain.Dto;
using EventServices.Services.Interfaces;

namespace EventServices.Controllers;

/// <summary>
/// Define los endpoints relacionados con el cambio de estado de eventos.
/// </summary>
public static class EventStatusEndpoints
{
    /// <summary>
    /// Mapea los endpoints para gestionar el estado de los eventos.
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>
    public static void MapRequestEventStatusEndpoints(this IEndpointRouteBuilder routes)
    {
        // Agrupa los endpoints bajo la ruta base /v1/assistances
        var group = routes.MapGroup("/v1/assistances").WithTags(nameof(RequestEventStatus));

        /// <summary>
        /// Actualiza el estado de un evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="input">Datos del nuevo estado.</param>
        /// <param name="_EventStatusChangeServices">Servicio de cambio de estado.</param>
        /// <returns>Resultado de la operación.</returns>
        group.MapPut("/events/{id}/statuses", UpdateStatusEventById);
        static async Task<IResult> UpdateStatusEventById(int id, RequestEventStatus input, IEventStatusChangeServices _EventStatusChangeServices)
        {
            try
            {
                var ResponseIdEvent = await _EventStatusChangeServices.UpdatedStatusOnEventAsync(id, input);
                return ResponseIdEvent != null ? TypedResults.Ok(ResponseIdEvent) : TypedResults.NotFound(ResponseIdEvent);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Marca un evento como completado.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="_EventStatusChangeServices">Servicio de cambio de estado.</param>
        /// <returns>Resultado de la operación.</returns>
        group.MapPatch("/events/{id}/complete", CompleteEventAsync);
        static async Task<IResult> CompleteEventAsync(int id, IEventStatusChangeServices _EventStatusChangeServices)
        {
            try
            {
                var result = await _EventStatusChangeServices.CompleteEventAsync(id);
                return result ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="_EventStatusChangeServices">Servicio de cambio de estado.</param>
        /// <returns>Resultado de la operación.</returns>
        group.MapPatch("/events/{id}/cancel", CancelEventAsync);
        static async Task<IResult> CancelEventAsync(int id, IEventStatusChangeServices _EventStatusChangeServices)
        {
            try
            {
                var result = await _EventStatusChangeServices.CancelEventAsync(id);
                return result ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="_EventStatusChangeServices">Servicio de cambio de estado.</param>
        /// <returns>Resultado de la operación.</returns>
        group.MapPatch("/events/{id}/reopen", ReOpenEventAsync);
        static async Task<IResult> ReOpenEventAsync(int id, IEventStatusChangeServices _EventStatusChangeServices)
        {
            try
            {
                var result = await _EventStatusChangeServices.ReOpenEventAsync(id);
                return result ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}