using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Actions;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar.Services.Implementations.Actions
{
    public class ActionService : IActionService
    {
        public void SetAction(Sprite sprite, Action action)
        {
            sprite.CurrentSequence = 0;
            sprite.TickAccumulation = 0;
            sprite.CurrentAction = action;
        }
    }
}
