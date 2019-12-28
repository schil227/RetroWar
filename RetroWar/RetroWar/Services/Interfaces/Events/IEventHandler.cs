using RetroWar.Models.Events;
using RetroWar.Models.Level;

namespace RetroWar.Services.Interfaces.Events
{
    public interface IEventHandler
    {
        bool HandleEvent(Stage stage, EventBase eventBase);
    }
}
