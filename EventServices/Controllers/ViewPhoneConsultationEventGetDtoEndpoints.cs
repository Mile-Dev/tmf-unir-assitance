using EventServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Objects;

namespace EventServices.Controllers;

/// <summary>
/// Clase estática que define los endpoints relacionados con la consulta telefónica de eventos médicos.
/// </summary>
public static class ViewPhoneConsultationEventGetDtoEndpoints
{
    /// <summary>
    /// Mapea los endpoints para la obtención de eventos de consulta telefónica.
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>
    public static void MapViewPhoneConsultationEventGetDtoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/medical-orientation/phone-consultation").WithTags("Event Phone Consultation");

        group.MapPost("/search/getlist", GetViewPhoneConsultationEventFilters);

        /// <summary>
        /// Obtiene una lista paginada de eventos de consulta telefónica según los filtros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda.</param>
        /// <param name="_ViewPhoneConsultationEventServices">Servicio de eventos de consulta telefónica.</param>
        /// <returns>Resultado HTTP con la lista paginada o error.</returns>
        static async Task<IResult> GetViewPhoneConsultationEventFilters([FromBody] Filters filters,
                                                        IViewPhoneConsultationEventsServices _ViewPhoneConsultationEventServices)
        {
            try
            {
                var result = await _ViewPhoneConsultationEventServices.GetEventPaginatedAsync(filters);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
