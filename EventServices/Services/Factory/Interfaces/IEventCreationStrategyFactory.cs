using EventServices.Services.Interfaces;

namespace EventServices.Services.Factory.Interfaces
{
    /// <summary>
    /// Fábrica para obtener la estrategia de creación de eventos adecuada según la clave del cliente.
    /// </summary>
    public interface IEventCreationStrategyFactory
    {
        /// <summary>
        /// Obtiene la estrategia de creación de eventos correspondiente al cliente especificado.
        /// </summary>
        /// <param name="clientKey">Clave única que identifica al cliente.</param>
        /// <returns>Instancia de <see cref="IEventCreationStrategy"/> asociada al cliente.</returns>
        IEventCreationStrategy GetStrategy(string clientKey);
    }
}
