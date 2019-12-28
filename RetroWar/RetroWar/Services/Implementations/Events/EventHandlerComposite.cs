using RetroWar.Models.Events;
using RetroWar.Models.Level;
using RetroWar.Services.Interfaces.Events;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Events
{
    public class EventHandlerComposite : IEventHandler
    {
        private readonly IEnumerable<IEventHandler> eventHandlers;

        public EventHandlerComposite(IEnumerable<IEventHandler> eventHandlers)
        {
            this.eventHandlers = eventHandlers;
        }

        public bool HandleEvent(Stage stage, EventBase eventBase)
        {
            foreach (var handler in eventHandlers)
            {
                if (handler.HandleEvent(stage, eventBase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
