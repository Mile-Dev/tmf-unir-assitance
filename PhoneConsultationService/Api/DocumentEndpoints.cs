using PhoneConsultationService.Common.Models;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Domain.Dto.Create;
using PhoneConsultationService.Domain.Dto.Query;
using PhoneConsultationService.Domain.Interfaces;

namespace PhoneConsultationService.Api;

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
    public static void MapS3ServiceEndpoints(this IEndpointRouteBuilder routes)
    {
        // Agrupa los endpoints bajo la ruta base y etiqueta correspondiente.
        var group = routes.MapGroup("/v1/medical-orientation/phone-consultation").WithTags("Upload Documents");

        /// <summary>
        /// Obtiene una URL prefirmada para subir un archivo a S3 asociado a un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <param name="filename">Nombre del archivo a subir.</param>
        /// <param name="contentype">Tipo de contenido del archivo.</param>
        /// <param name="servicess3">Servicio de almacenamiento S3.</param>
        /// <returns>Respuesta con la URL prefirmada o error.</returns>
        group.MapPost("events/{id}/presigned-uploads", async (int id, DocumentUploadDto documentUploadDto, IStorageS3Services servicess3) =>
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
        group.MapPost("events/{id}/presigned-downloads", async (int id, DocumentDownloadDto documentDownloadDto, IStorageS3Services servicess3) =>
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
    }
}
