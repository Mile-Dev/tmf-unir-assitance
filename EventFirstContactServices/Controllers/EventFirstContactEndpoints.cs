using EventFirstContactServices.Domain.Dto;
using EventFirstContactServices.Services;

namespace EventFirstContactServices.Controllers;

public static class EventFirstContactEndpoints
{
    public static void MapEventFirstContactCreateDtoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/assist/event/firstcontact").WithTags(nameof(EventFirstContactCreateDto));

        group.MapGet("/all", GetEventFirstcontactDraft);
        static async Task<IResult?> GetEventFirstcontactDraft(IEventFirstContactServices _ieventfirstcontactservices)
        {
            try
            {
                var result = await _ieventfirstcontactservices.GetEventAllRecordAsync(100);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }        

        group.MapGet("/{id}", GetContactByIdEventFirstcontact);
        static async Task<IResult?> GetContactByIdEventFirstcontact(string id, IEventFirstContactServices _ieventfirstcontactservices)
        {
            try
            {
                var result = await _ieventfirstcontactservices.GetEventFirstContactByIdAsync(id);
                return !string.IsNullOrEmpty(result.Event.Id) ? TypedResults.Ok(result) : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPost("/step", CreatedEventFirstContact);
        static async Task<IResult> CreatedEventFirstContact(EventFirstContactCreateDto input, IEventFirstContactServices _ieventfirstcontactservices)
        {
            try
            {
                var resul = await _ieventfirstcontactservices.CreateUpdateEventFirstContactAsync(input);
                return string.IsNullOrEmpty(resul.Id) ? TypedResults.NotFound()
                                                              : TypedResults.Created($"/api/event/{resul.Id}", resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/{id}", DeleteEventFirstContactByIdProvider);
        static async Task<IResult> DeleteEventFirstContactByIdProvider(string id, IEventFirstContactServices _ieventfirstcontactservices)
        {
            try
            {
                var resul = await _ieventfirstcontactservices.DeleteEventFirstContactAsync(id);
                return !resul ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
