using EventStatusSwitchTempServices.Domain.Dto;
using EventStatusSwitchTempServices.Services;

namespace EventStatusSwitchTempServices.Controllers;

public static class StatusEventEndpoints
{
    public static void MapRequestUpdatedStatusEventEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/temp/assist").WithTags(nameof(RequestEventStatus));

        group.MapPost("/update/status", UpdateStatusEventById);
        static async Task<IResult> UpdateStatusEventById(RequestEventStatus ObjectUpdatedStatusEvent, IEventStatusSwitchServices _EventStatusSwitch)
        {
            try
            {
                var ResponseIdEvent = await _EventStatusSwitch.UpdateStatusEventById(ObjectUpdatedStatusEvent);
                return ResponseIdEvent.Success ? TypedResults.Ok(ResponseIdEvent) : TypedResults.BadRequest(ResponseIdEvent);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }        
    }
}
