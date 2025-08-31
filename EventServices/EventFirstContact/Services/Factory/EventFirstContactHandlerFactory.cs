using EventServices.EventFirstContact.Services.Factory.Interfaces;
using EventServices.EventFirstContact.Services.Strategy.Interfaces;

namespace EventServices.EventFirstContact.Services.Factory
{
    public class EventFirstContactHandlerFactory : IEventFirstContactHandlerFactory
    {
        private readonly Dictionary<string, IEventFirstContactHandler> _handlers;

        public EventFirstContactHandlerFactory(IEnumerable<IEventFirstContactHandler> handlers)
        {
            Console.WriteLine($"Se encontraron {handlers.Count()} handlers");

            _handlers = handlers.ToDictionary(h => h.Screen);
        }


        public IEnumerable<IEventFirstContactHandler> GetAllHandlers()
        {
            return _handlers.Values;
        }

        public IEventFirstContactHandler GetHandler(string screen)
        {
            if (!_handlers.TryGetValue(screen, out IEventFirstContactHandler? value))
                throw new ArgumentException($"Handler not found for screen {screen}");

            return value;
        }
    }
}
