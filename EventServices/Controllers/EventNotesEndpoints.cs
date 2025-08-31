using EventServices.Domain.Dto;
using EventServices.Services.Interfaces;

namespace EventServices.Controllers;

public static class EventNotesEndpoints
{
    public static void MapResquestLogDataEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Notes");

        group.MapPost("/notes", CreatedNotes);
        static async Task<IResult> CreatedNotes(RequestLogData input, INotesSqsServices _notesSqsServices)
        {
            try
            {
                await _notesSqsServices.SendMessageAsync(input);
                return TypedResults.Ok(new { message = "Mensaje enviado exitosamente a SQS" });
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
