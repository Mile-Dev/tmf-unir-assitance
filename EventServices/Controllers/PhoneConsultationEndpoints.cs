using EventServices.Common;
using EventServices.Common.Helpers;
using EventServices.Common.Models;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Entities;
using EventServices.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Objects;

namespace EventServices.Controllers;

/// <summary>
/// Define los endpoints relacionados con las consultas telefónicas asociadas a eventos.
/// </summary>
public static class PhoneConsultationEndpoints
{
    /// <summary>
    /// Mapea los endpoints de consultas telefónicas al grupo de rutas "/v1/assistances".
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>
    public static void MapPhoneConsultationEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags(nameof(PhoneConsultation));

        /// <summary>
        /// Obtiene la lista de consultas telefónicas asociadas a un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="_phoneConsultationServices">Servicio de consultas telefónicas.</param>
        /// <returns>Lista de consultas telefónicas o NotFound si no existen.</returns>
        group.MapGet("/events/{id}/phone-consultations", GetPhoneConsultationByEvent);

        static async Task<IResult> GetPhoneConsultationByEvent(int id, IPhoneConsultationServices _phoneConsultationServices)
        {
            try
            {
                var result = await _phoneConsultationServices.GetListPhoneConsultationsByEventAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva consulta telefónica programada para un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="input">Datos de la consulta telefónica.</param>
        /// <param name="_phoneConsultationServices">Servicio de consultas telefónicas.</param>
        /// <returns>Resultado de la creación o NotFound si falla.</returns>
        group.MapPost("/events/{id}/phone-consultations", CreatedPhoneConsultationByEvent);
        static async Task<IResult> CreatedPhoneConsultationByEvent(
            int id, 
            PhoneConsultationDto input, 
            IPhoneConsultationServices _phoneConsultationServices,
            IValidator<PhoneConsultationDto> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return ValidationHelper.HandleValidationFailure(
                        validationResult,
                        EnumError.PHONE_CONSULTATION_VALIDATION_FAILED.ToString(),
                        "Errores de validación al crear el phone consultation."
                    );
                }

                var result = await _phoneConsultationServices.CreatedScheduledPhoneConsultationAsync(id, input);
                OperationSuccessResponse<ResponseCreatedDto> successResponse = new(result);
                return Results.Ok(successResponse);               
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return Results.BadRequest(errorDetails);
            }
        }

        /// <summary>
        /// Cierra una consulta telefónica programada de un evento.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <param name="input">Datos de cierre de la consulta.</param>
        /// <param name="_phoneConsultationServices">Servicio de consultas telefónicas.</param>
        /// <returns>Ok si se cierra correctamente, NotFound si no existe.</returns>
        group.MapPatch("/events/phone-consultations/{id}/close", ClosePhoneConsultationByEvent);

        static async Task<IResult> ClosePhoneConsultationByEvent(int id,  IPhoneConsultationServices _phoneConsultationServices)
        {
            try
            {
                var result = await _phoneConsultationServices.ClosedScheduledPhoneConsultationAsync(id);
                return !result ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cierra una consulta telefónica programada de un evento.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <param name="input">Datos de cierre de la consulta.</param>
        /// <param name="_phoneConsultationServices">Servicio de consultas telefónicas.</param>
        /// <returns>Ok si se cierra correctamente, NotFound si no existe.</returns>
        group.MapPatch("/events/phone-consultations/{id}/cancel", CanceledPhoneConsultationByEvent);

        static async Task<IResult> CanceledPhoneConsultationByEvent(int id, IPhoneConsultationServices _phoneConsultationServices)
        {
            try
            {
                var result = await _phoneConsultationServices.CanceledScheduledPhoneConsultationAsync(id);
                return !result ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene una lista paginada de eventos de consultas telefónicas aplicando filtros.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda y paginación.</param>
        /// <param name="_ViewPhoneConsultationEventServices">Servicio de vista de eventos de consultas telefónicas.</param>
        /// <returns>Lista paginada de eventos o NotFound si no existen.</returns>
        group.MapPost("/events/phone-consultations", GetViewPhoneConsultationEventFilters);

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


        group.MapPatch("/events/phone-consultations/{id}/reschedule", RescheduledEventProviderAsync);
        static async Task<IResult> RescheduledEventProviderAsync(int id, ReschedulePhoneConsultationDto reschedulePhoneConsultation, IPhoneConsultationServices _phoneConsultationServices)
        {
            try
            {
                var result = await _phoneConsultationServices.ReScheduledPhoneConsultationAsync(id, reschedulePhoneConsultation);
                return !result ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            { 
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
