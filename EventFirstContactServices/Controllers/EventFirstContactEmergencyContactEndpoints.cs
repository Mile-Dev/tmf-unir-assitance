using EventFirstContactServices.Domain.Dto.Get;
using EventFirstContactServices.Services;

namespace EventFirstContactServices.Controllers;

public static class EventFirstContactEmergencyContactEndpoints
{
    public static void MapEventFirstContactEmergencyContactGetDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/assist/event/firstcontact").WithTags(nameof(EventFirstContactEmergencyContactGetDto));

        group.MapGet("/contactemergency/{id}", GetContactEmergenciByIdEventFirstcontact);
        static async Task<IResult?> GetContactEmergenciByIdEventFirstcontact(string id, IEventContactEmergencyContactServices _ieventContactEmergencyContactServices)
        {
            try
            {
                var result = await _ieventContactEmergencyContactServices.GetEventFirstContactEmergencyContactGetDtoByIdAsync(id);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
