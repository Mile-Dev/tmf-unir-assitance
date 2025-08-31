namespace EventServices.Services.Interfaces.Security
{
    /// <summary>
    /// Define un contrato para mapear claves API a identificadores de cliente.
    /// </summary>
    public interface IApiKeyClientMapper
    {
        /// <summary>
        /// Resuelve el identificador de cliente asociado a una clave API dada.
        /// </summary>
        /// <param name="apiKey">Clave API proporcionada por el cliente.</param>
        /// <returns>
        /// El identificador de cliente correspondiente si existe; de lo contrario, <c>null</c>.
        /// </returns>
        string? ResolveClientKey(string apiKey);
    }
}
