using PhoneConsultationService.Domain.Dto.Create;
using PhoneConsultationService.Domain.Dto.Query;

namespace PhoneConsultationService.Domain.Interfaces
{
    /// <summary>
    /// Define los métodos para interactuar con servicios de almacenamiento S3,
    /// permitiendo la generación de URLs prefirmadas para subir y descargar archivos.
    /// </summary>
    public interface IStorageS3Services
    {
        /// <summary>
        /// Genera una URL prefirmada para subir un archivo a S3.
        /// </summary>
        /// <param name="filename">Nombre del archivo a subir.</param>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="contentType">Tipo de contenido del archivo.</param>
        /// <returns>URL prefirmada para la subida del archivo.</returns>
        Task<string> GetPresignedUploadUrlAsync(int eventNumber, DocumentUploadDto documentUploadDto);

        /// <summary>
        /// Genera una URL prefirmada para descargar un archivo desde S3.
        /// </summary>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="filename">Nombre del archivo a descargar.</param>
        /// <returns>URL prefirmada para la descarga del archivo.</returns>
        Task<string> GetPresignedDownloadUrlAsync(int eventNumber, DocumentDownloadDto documentDownloadDto);
    }
}
