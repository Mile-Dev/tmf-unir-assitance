using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Services;

namespace MasterRdsServices.Controllers;

public static class EventStatusEndpoints
{
    public static void MapEventStatusQueryDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("Event Statuses");

        group.MapGet("/event-statuses", GetEventStatus);
        static IResult GetEventStatus(IEventStatusServices _IEventStatusServices)
        {
            try
            {
                var result = _IEventStatusServices.GetEventStatusAsync(); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
