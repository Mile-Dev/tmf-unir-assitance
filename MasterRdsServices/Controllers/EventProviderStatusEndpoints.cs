using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace MasterRdsServices.Controllers;

public static class EventProviderStatusEndpoints
{
    public static void MapEventProviderStatusQueryDtoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/masters").WithTags("Event Provider Statuses");


        group.MapGet("/eventprovider-statuses", GetEventProviderStatus);
        static IResult GetEventProviderStatus(IEventProviderStatusServices _IEventProviderStatusServices)
        {
            try
            {
                var result = _IEventProviderStatusServices.GetEventProviderStatus(); ;
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
