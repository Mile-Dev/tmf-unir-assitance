using IssuanceMokServices.Domain.Dto;
using IssuanceMokServices.Services;

namespace IssuanceMokServices.Controllers;

public static class UploadEndpoints
{
    public static void MapUploadRequestEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/issuances").WithTags(nameof(UploadRequest));

        group.MapPost("/", SaveDocument);
        static async Task<IResult?> SaveDocument( UploadRequest uploadRequest, IIssuanceMokServices _iissuanceMokServices)
        {
            try
            {
                var result = await _iissuanceMokServices.GetPresignUrlIssuanceAsync(uploadRequest);
                return result == null ? TypedResults.NotFound(): TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapGet("/{filename}", GetDocument);
        static async Task<IResult?> GetDocument(string filename, IIssuanceMokServices _iissuanceMokServices)
        {
            try
            {
                var result = await _iissuanceMokServices.DownloadUrllssuanceAsync(filename);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }

        group.MapPut("/{id}", UpdateDocument);
         static async Task<IResult?> UpdateDocument(string id, UploadRequest uploadRequest, IIssuanceMokServices _iissuanceMokServices)
        {
            try
            {
                var result = await _iissuanceMokServices.UpdateDocumentIssuanceAsync(id, uploadRequest);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }
        }


        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new UploadRequest { ID = id });
        })
        .WithName("DeleteUploadRequest")
        .WithOpenApi();
    }
}
