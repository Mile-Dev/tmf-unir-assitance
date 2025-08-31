using EventServices.Common.Models;
using FluentValidation.Results;

namespace EventServices.Common.Helpers
{
    /// <summary>
    /// Proporciona métodos auxiliares para manejar errores de validación en la aplicación.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Procesa los errores de validación y construye una respuesta de error estructurada para el cliente.
        /// </summary>
        /// <param name="result">El resultado de la validación que contiene los errores.</param>
        /// <param name="customCode">Código de error personalizado (opcional).</param>
        /// <param name="customMessage">Mensaje de error personalizado (opcional).</param>
        /// <returns>Una respuesta BadRequest con los detalles de los errores de validación.</returns>
        public static IResult HandleValidationFailure(ValidationResult result, string? customCode = null, string? customMessage = null)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new OperationErrorsResponse(string.IsNullOrEmpty(customCode) ? "VALIDATION_FAILED" : customCode,
                                                       string.IsNullOrEmpty(customMessage) ? "Errores de validación detectados." : customMessage,
                                                       errors);

            return TypedResults.BadRequest(response);
        }
    }
}
