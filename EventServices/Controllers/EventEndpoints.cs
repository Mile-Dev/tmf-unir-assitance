using EventServices.Common;
using EventServices.Common.Helpers;
using EventServices.Common.Models;
using EventServices.Domain.Dto;
using EventServices.Services.Factory.Interfaces;
using EventServices.Services.Interfaces;
using EventServices.Services.Interfaces.Security;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Objects;

namespace EventServices.Controllers;

/// <summary>
/// Clase estática que define los endpoints relacionados con la gestión de eventos.
/// </summary>
public static class EventEndpoints
{
    /// <summary>
    /// Mapea los endpoints para la creación, consulta, actualización y búsqueda de eventos.
    /// </summary>
    /// <param name="routes">El constructor de rutas de endpoints.</param>
    public static void MapEventDraftCreateEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Event");

        group.MapGet("/events/{id}", GetViewEvent);
        /// <summary>
        /// Obtiene los detalles de un evento por su identificador.
        /// </summary>
        /// <param name="context">Contexto HTTP.</param>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="apiKeyClientMapper">Servicio para mapear la API Key al cliente.</param>
        /// <param name="_EventCreationStrategyFactory">Fábrica de estrategias de creación de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> GetViewEvent(HttpContext context, int id, IApiKeyClientMapper apiKeyClientMapper, IEventCreationStrategyFactory _EventCreationStrategyFactory)
        {
            try
            {
                var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                    return TypedResults.Unauthorized();

                var clientKey = apiKeyClientMapper.ResolveClientKey(apiKey) ?? "default";
                var strategy = _EventCreationStrategyFactory.GetStrategy(clientKey);

                var result = await strategy.GetEventAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/events/by-code/{eventCode}", GetEventBycode);
        /// <summary>
        /// Obtiene los detalles de un evento por su código.
        /// </summary>
        /// <param name="context">Contexto HTTP.</param>
        /// <param name="eventCode">Código del evento.</param>
        /// <param name="apiKeyClientMapper">Servicio para mapear la API Key al cliente.</param>
        /// <param name="_EventCreationStrategyFactory">Fábrica de estrategias de creación de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> GetEventBycode(HttpContext context, string eventCode, IApiKeyClientMapper apiKeyClientMapper, IEventCreationStrategyFactory _EventCreationStrategyFactory)
        {
            try
            {
                var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                    return TypedResults.Unauthorized();

                var clientKey = apiKeyClientMapper.ResolveClientKey(apiKey) ?? "default";
                var strategy = _EventCreationStrategyFactory.GetStrategy(clientKey);

                var result = await strategy.GetEventByCodeAsync(eventCode);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/events/by-voucher/{voucherNumber}", GetEventByVoucher);
        /// <summary>
        /// Obtiene los detalles de un evento por el número de voucher.
        /// </summary>
        /// <param name="context">Contexto HTTP.</param>
        /// <param name="voucherNumber">Número de voucher.</param>
        /// <param name="apiKeyClientMapper">Servicio para mapear la API Key al cliente.</param>
        /// <param name="_EventCreationStrategyFactory">Fábrica de estrategias de creación de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> GetEventByVoucher(HttpContext context, string voucherNumber, IApiKeyClientMapper apiKeyClientMapper, IEventCreationStrategyFactory _EventCreationStrategyFactory)
        {
            try
            {
                var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                    return TypedResults.Unauthorized();

                var clientKey = apiKeyClientMapper.ResolveClientKey(apiKey) ?? "default";
                var strategy = _EventCreationStrategyFactory.GetStrategy(clientKey);

                var result = await strategy.GetEventByVoucherAsync(voucherNumber);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/events", CreatedEventAsync);
        /// <summary>
        /// Crea un nuevo evento.
        /// </summary>
        /// <param name="context">Contexto HTTP.</param>
        /// <param name="input">Datos del evento a crear.</param>
        /// <param name="apiKeyClientMapper">Servicio para mapear la API Key al cliente.</param>
        /// <param name="_EventCreationStrategyFactory">Fábrica de estrategias de creación de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> CreatedEventAsync(
            HttpContext context,
            RequestEvent input,
            IApiKeyClientMapper apiKeyClientMapper,
            IEventCreationStrategyFactory _EventCreationStrategyFactory,
            IValidator<RequestEvent> validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(input);
                if (!validationResult.IsValid)
                {
                    return ValidationHelper.HandleValidationFailure(
                        validationResult,
                        EnumError.EVENT_VALIDATION_FAILED.ToString(),
                        "Errores de validación al crear el evento."
                    );
                }

                var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                    return TypedResults.Unauthorized();

                var clientKey = apiKeyClientMapper.ResolveClientKey(apiKey) ?? "default";
                var strategy = _EventCreationStrategyFactory.GetStrategy(clientKey);

                var ResponseIdEvent = await strategy.CreateEventAsync(input);
                return ResponseIdEvent.Id < 0 ? TypedResults.NotFound()
                                       : TypedResults.Created($"/api/event/{ResponseIdEvent}", ResponseIdEvent);
            }
            catch (Exception ex)
            {
                var validationResult = new OperationErrorsResponse(EnumError.EVENT_CREATION_FAILED.ToString(), "Error al crear el evento.",
                     new Dictionary<string, string[]> { { "General", new[] { ex.Message } } });                
                return TypedResults.BadRequest(validationResult);
            }
        }

        group.MapPost("/events/search", GetViewEventFilters);
        /// <summary>
        /// Busca eventos aplicando filtros y paginación.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda y parámetros de paginación.</param>
        /// <param name="_ViewEventServices">Servicio de consulta de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> GetViewEventFilters([FromBody] Filters filters, IViewEventsServices _ViewEventServices)
        {
            try
            {
                var result = await _ViewEventServices.GetEventPaginatedAsync(filters);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/events/{id}", UpdateEventAsync);
        /// <summary>
        /// Actualiza los datos de un evento existente.
        /// </summary>
        /// <param name="context">Contexto HTTP.</param>
        /// <param name="id">Identificador del evento a actualizar.</param>
        /// <param name="input">Datos actualizados del evento.</param>
        /// <param name="apiKeyClientMapper">Servicio para mapear la API Key al cliente.</param>
        /// <param name="_EventCreationStrategyFactory">Fábrica de estrategias de creación de eventos.</param>
        /// <returns>Resultado de la operación.</returns>
        static async Task<IResult> UpdateEventAsync(HttpContext context, int id, RequestUpdatedEvent input, IApiKeyClientMapper apiKeyClientMapper, IEventCreationStrategyFactory _EventCreationStrategyFactory)
        {
            try
            {
                var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                    return TypedResults.Unauthorized();

                var clientKey = apiKeyClientMapper.ResolveClientKey(apiKey) ?? "default";
                var strategy = _EventCreationStrategyFactory.GetStrategy(clientKey);

                var result = await strategy.UpdateEventAsync(id, input);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
