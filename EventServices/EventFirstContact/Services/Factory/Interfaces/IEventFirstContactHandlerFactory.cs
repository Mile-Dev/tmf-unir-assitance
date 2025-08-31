using EventServices.EventFirstContact.Services.Strategy.Interfaces;

namespace EventServices.EventFirstContact.Services.Factory.Interfaces
{
    public interface IEventFirstContactHandlerFactory
    {
        IEventFirstContactHandler GetHandler(string screen);
        IEnumerable<IEventFirstContactHandler> GetAllHandlers();
    }
}
