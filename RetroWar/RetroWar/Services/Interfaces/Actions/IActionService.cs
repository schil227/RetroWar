using RetroWar.Models.Sprites;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar.Services.Interfaces.Actions
{
    public interface IActionService
    {
        void SetAction(Sprite sprite, Action action);
    }
}
