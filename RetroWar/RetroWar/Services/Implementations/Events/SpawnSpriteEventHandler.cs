using RetroWar.Models.Events;
using RetroWar.Models.Level;
using RetroWar.Services.Interfaces.Events;

namespace RetroWar.Services.Implementations.Events
{
    public class SpawnSpriteEventHandler : IEventHandler
    {
        public SpawnSpriteEventHandler()
        {

        }

        public bool HandleEvent(Stage stage, EventBase eventBase)
        {
            var createSpriteEvent = eventBase as SpawnSpriteEvent;

            if (createSpriteEvent == null)
            {
                return false;
            }


            return true;
        }
    }
}
