using EventServices.Services.Factory.Interfaces;
using EventServices.Services.Interfaces;

namespace EventServices.Services.Factory
{
    /// <summary>
    /// Fábrica para obtener la estrategia de creación de eventos adecuada según la clave del cliente.
    /// Implementa el patrón Strategy para seleccionar dinámicamente la lógica de creación de eventos
    /// específica para cada cliente, permitiendo así la extensión y personalización del proceso de creación
    /// de eventos sin modificar el código cliente.
    /// </summary>
    public class EventCreationStrategyFactory(IEnumerable<IEventCreationStrategy> strategies) : IEventCreationStrategyFactory
    {
        
        
// Diccionario que almacena las estrategias de creación de eventos, indexadas por la clave del cliente.
        /// Permite la búsqueda eficiente de la estrategia correspondiente utilizando una comparación de cadenas
        /// que no distingue entre mayúsculas y minúsculas.
        /// </summary>
        private readonly Dictionary<string, IEventCreationStrategy> _strategies = strategies.ToDictionary(s => s.ClientKey,
                                                                                                          s => s,
                                                                                                          StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Obtiene la estrategia de creación de eventos correspondiente a la clave del cliente proporcionada.
        /// Si no existe una estrategia específica para el cliente, retorna la estrategia por defecto.
        /// </summary>
        /// <param name="clientKey">Clave única del cliente.</param>
        /// <returns>Instancia de <see cref="IEventCreationStrategy"/> asociada al cliente.</returns>
        public IEventCreationStrategy GetStrategy(string clientKey)
              => _strategies.TryGetValue(clientKey, out var strategy)
                  ? strategy
                  : _strategies["default"];
    }
}
