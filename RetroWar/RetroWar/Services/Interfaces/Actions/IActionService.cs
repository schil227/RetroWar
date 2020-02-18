using RetroWar.Models.Sprites;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar.Services.Interfaces.Actions
{
    public interface IActionService
    {
        void RemoveAction(Sprite sprite, Action action);
        void SetAction(Sprite sprite, Action newAction, Action? actionToRemove = null);
        void ProcessActionEvent(Sprite sprite, Action action);
    }
}
