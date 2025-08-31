using Amazon.DynamoDBv2.Model;
using EventServices.Common;
using EventServices.Common.Helpers;
using EventServices.Common.Models;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Services.Interfaces;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EventServices.Controllers;

/// <summary>
/// Define los endpoints relacionados con los proveedores de eventos.
/// </summary>
public static class EventProviderEndpoints
{
    /// <summary>
    /// Mapea los endpoints para la gestión de proveedores de eventos.
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>
    public static void MapEventProviderDtoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Event Provider");

        /// <summary>
        /// Obtiene un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Proveedor de evento o NotFound si no existe.</returns>
        group.MapGet("/events/providers/{id}", GetEventProviderByIdAsync);
        static async Task<IResult> GetEventProviderByIdAsync(int id, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var result = await _EventProviderServices.GetEventProviderByIdAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene los proveedores asociados a un evento específico.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Lista de proveedores o NotFound si no existen.</returns>
        group.MapGet("/events/{id}/providers", GetEventProviderByIdEventAsync);
        static async Task<IResult> GetEventProviderByIdEventAsync(int id, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var result = await _EventProviderServices.GetEventProviderByEventIdAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="input">Datos actualizados del proveedor.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Proveedor actualizado o NotFound si no existe.</returns>
        group.MapPut("/events/providers/{id}", UpdateEventProviderByIdAsync);
        static async Task<IResult> UpdateEventProviderByIdAsync(int id, EventProviderDto input, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var updated = await _EventProviderServices.UpdatedEventProviderAsync(id, input);
                var resultOperation = new OperationSuccessResponse<Domain.Dto.Query.EventProviderDto>(updated);
                return updated is not null ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo proveedor de evento.
        /// </summary>
        /// <param name="input">Datos del nuevo proveedor.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Proveedor creado o NotFound si falla la creación.</returns>
        group.MapPost("/events/providers", CreatedEventProviderByIdAsync);
        static async Task<IResult> CreatedEventProviderByIdAsync(EventProviderDto input, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var result = await _EventProviderServices.CreatedEventProviderAsync(input);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Ok si se elimina, NotFound si no existe.</returns>
        group.MapDelete("/events/providers/{id}", DeleteEventProviderAsync);
        static async Task<IResult> DeleteEventProviderAsync(int id, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var result = await _EventProviderServices.DeletedEventProviderByIdAsync(id);
                var resultOperation = new OperationSuccessResponse<bool>(result);
                return result ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }

        /// <summary>
        /// Cancela un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Ok si se cancela, NotFound si no existe.</returns>
        group.MapPatch("/events/providers/{id}/cancel", CancelEventProviderAsync);
        static async Task<IResult> CancelEventProviderAsync(int id, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var updated = await _EventProviderServices.CanceledEventProviderByIdAsync(id);
                var resultOperation = new OperationSuccessResponse<Domain.Dto.Query.EventProviderDto>(updated);
                return updated is not null ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Marca como completado un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Ok si se completa, NotFound si no existe.</returns>
        group.MapPatch("/events/providers/{id}/complete", CompleteEventProviderAsync);
        static async Task<IResult> CompleteEventProviderAsync(int id, IEventProviderServices _EventProviderServices)
        {
            try
            {
                var updated = await _EventProviderServices.CompletedEventProviderByIdAsync(id);
                var resultOperation = new OperationSuccessResponse<Domain.Dto.Query.EventProviderDto>(updated);
                return updated is not null ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }

        /// <summary>
        /// Reagenda un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="rescheduleEventProvider">Datos de reagendamiento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Ok si se reagenda, NotFound si no existe.</returns>
        group.MapPatch("/events/providers/{id}/reschedule", RescheduledEventProviderAsync);
        static async Task<IResult> RescheduledEventProviderAsync(int id, RescheduleEventProviderDto rescheduleEventProvider, IEventProviderServices _EventProviderServices, IValidator<RescheduleEventProviderDto> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(rescheduleEventProvider);
                if (!validationResult.IsValid)
                {
                    return ValidationHelper.HandleValidationFailure(
                        validationResult,
                        EnumError.EVENT_VALIDATION_FAILED.ToString(),
                        "Errores de validación al reagendar el evento proveedor."
                    );
                }
                var updated = await _EventProviderServices.RescheduledEventProviderByIdAsync(id, rescheduleEventProvider);
                var resultOperation = new OperationSuccessResponse<Domain.Dto.Query.EventProviderDto>(updated);
                return updated is not null ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }

        /// <summary>
        /// Reagenda un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="rescheduleEventProvider">Datos de reagendamiento.</param>
        /// <param name="_EventProviderServices">Servicio de proveedores de eventos.</param>
        /// <returns>Ok si se reagenda, NotFound si no existe.</returns>
        group.MapPatch("/events/providers/{id}/diagnostic", DiagnosticEventProviderAsync);
        static async Task<IResult> DiagnosticEventProviderAsync(int id, RequestUpdateDiagnosticDto diagnosticEventProvider, IEventProviderServices _EventProviderServices, IValidator<RequestUpdateDiagnosticDto> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(diagnosticEventProvider);
                if (!validationResult.IsValid)
                {
                    return ValidationHelper.HandleValidationFailure(
                        validationResult,
                        EnumError.EVENT_VALIDATION_FAILED.ToString(),
                        "Errores de validación al crear el evento."
                    );
                }
                var updated = await _EventProviderServices.UpdateDiagnosticEventProviderByIdAsync(id, diagnosticEventProvider);
                var resultOperation = new OperationSuccessResponse<Domain.Dto.Query.EventProviderDto>(updated);
                return updated is not null ? TypedResults.Ok(resultOperation) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }
    }
}
