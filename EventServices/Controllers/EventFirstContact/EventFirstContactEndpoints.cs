using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;
using EventServices.EventFirstContact.Services.Interfaces;

namespace EventServices.Controllers.EventFirstContact;

public static class EventFirstContactEndpoints
{
    public static void MapResponseFirstContactEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Event First Contact");

        group.MapPost("/events/firstcontacts", CreatedEventFirstContact);
        static async Task<IResult> CreatedEventFirstContact(EventFirstContactDto input, IEventFirstContactServices _eventfirstcontactservices)
        {
            try
            {
                var resul = await _eventfirstcontactservices.CreateUpdateEventFirstContactAsync(input);
                return string.IsNullOrEmpty(resul.Id) ? TypedResults.NotFound()
                                                      : TypedResults.Created($"/api/event/{resul.Id}", resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapDelete("/events/firstcontacts/{id}", DeletedEventFirstContact);
        static async Task<IResult> DeletedEventFirstContact(string id, IEventFirstContactServices _eventfirstcontactservices)
        {
            try
            {
                var resul = await _eventfirstcontactservices.DeleteEventFirstContactAsync(id);
                return resul ? TypedResults.Ok() : TypedResults.NotFound();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/events/firstcontacts/{id}", GetByIdEventFirstContact);
        static async Task<IResult> GetByIdEventFirstContact(string id, IEventFirstContactServices _eventfirstcontactservices)
        {
            try
            {
                var resul = await _eventfirstcontactservices.GetEventFirstContactByIdAsync(id);
                return string.IsNullOrEmpty(resul.Id) ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/events/firstcontacts/{id}/contactemergencies", GetByIdEventFirstContactEmergencies);
        static async Task<IResult> GetByIdEventFirstContactEmergencies(string id, IEventFirstContactServices _eventfirstcontactservices)
        {
            try
            {
                var resul = await _eventfirstcontactservices.GetEventFirstContactEmergenciesByIdAsync(id);
                return string.IsNullOrEmpty(resul.Id) ? TypedResults.NotFound() : TypedResults.Ok(resul);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
