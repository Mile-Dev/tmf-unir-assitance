using EventServices.Common.Models;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Objects;

namespace EventServices.Controllers;

/// <summary>
/// Define los endpoints para la gestión de documentos relacionados con consultas telefónicas médicas,
/// permitiendo la obtención de URLs prefirmadas para la subida y descarga de archivos en S3.
/// </summary>
public static class DocumentEndpoints
{
    /// <summary>
    /// Mapea los endpoints relacionados con el servicio S3 para la subida y descarga de documentos.
    /// </summary>
    /// <param name="routes">Constructor de rutas de endpoints.</param>

    public static void MapS3ServiceEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/v1/assistances").WithTags("Upload Documents");

        /// <summary>
        /// Obtiene una URL prefirmada para subir un archivo a S3 asociado a un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="filename">Nombre del archivo a subir.</param>
        /// <param name="contentype">Tipo de contenido del archivo.</param>
        /// <param name="servicess3">Servicio de almacenamiento S3.</param>
        /// <returns>Respuesta con la URL prefirmada o error.</returns>
        group.MapPost("events/{id}/presigned-uploads", async (int id, DocumentUploadDto documentUploadDto, IDocumentServices servicess3) =>
        {
            try
            {
                var resultPresignedUpload = await servicess3.GetPresignedUploadUrlAsync(id, documentUploadDto);
                OperationSuccessResponse<string> successResponse = new(resultPresignedUpload);
                return Results.Ok(successResponse);
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return Results.BadRequest(errorDetails);
            }
        })
        .WithName("GetS3ServicePresigned-upload")
        .WithOpenApi();

        /// <summary>
        /// Obtiene una URL prefirmada para descargar un archivo desde S3 asociado a un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="fileName">Nombre del archivo a descargar.</param>
        /// <param name="servicess3">Servicio de almacenamiento S3.</param>
        /// <returns>Respuesta con la URL prefirmada o error.</returns>
        group.MapPost("events/{id}/presigned-downloads", async (int id, DocumentDownloadDto documentDownloadDto, IDocumentServices servicess3) =>
        {
            try
            {
                var resultPresignedUpload = await servicess3.GetPresignedDownloadUrlAsync(id, documentDownloadDto);
                OperationSuccessResponse<string> successResponse = new(resultPresignedUpload);
                return Results.Ok(successResponse);
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return Results.BadRequest(errorDetails);
            }
        })
        .WithName("GetS3ServicePresigned-download")
        .WithOpenApi();

        /// <summary>
        /// Crea un nuevo proveedor de evento.
        /// </summary>
        /// <param name="input">Datos del nuevo documento.</param>
        /// <param name="_documentServices">Servicio de documentos.</param>
        /// <returns>Documento creado o NotFound si falla la creación.</returns>
        group.MapPost("/events/documents", CreatedDocumentByIdAsync);
        static async Task<IResult> CreatedDocumentByIdAsync(DocumentCreatedDto input, IDocumentServices _documentServices)
        {
            try
            {
                var result = await _documentServices.CreatedDocumentAsync(input);
                OperationSuccessResponse<ResponseCreatedDto> successResponse = new(result);
                return result is null ? TypedResults.NotFound() : TypedResults.Ok(successResponse);
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }


        /// <summary>
        /// Crea un nuevo proveedor de evento.
        /// </summary>
        /// <param name="id">Datos del nuevo documento.</param>
        /// <param name="filter">Servicio de documentos.</param>         
        /// <param name="_documentServices">Servicio de documentos.</param>
        /// <returns>Documento creado o NotFound si falla la creación.</returns>
        group.MapGet("/events/{id}/documents", GetListEventProviderByIdAsync);
        static async Task<IResult> GetListEventProviderByIdAsync([AsParameters] ParameterGetList filter, [FromRoute] int id, IDocumentServices _documentServices)
        {
            try
            {
                var result = await _documentServices.GetListDocumentByEventIdAsync(id, filter);
                return result == null ? TypedResults.NotFound() : TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                OperationErrorsResponse errorDetails = new("500", "Bad Request", ex.Message);
                return TypedResults.BadRequest(errorDetails);
            }
        }
    }
}
