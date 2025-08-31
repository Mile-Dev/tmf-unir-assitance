using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using TrackingMokServices.Domain.Dto;
namespace TrackingMokServices.Controllers;

public static class RequestMokEndpoints
{
    public static void MapRequestMokEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/requestmok").WithTags(nameof(RequestMok));


        group.MapPost("/execute", ExecuteEventMok);
        static IResult ExecuteEventMok(RequestMok input)
        {
            try
            {
                return !true ? TypedResults.NotFound() : TypedResults.Ok();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

    }
}