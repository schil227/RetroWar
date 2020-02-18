using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;

namespace RetroWar.Services.Interfaces.Actions
{
    public interface ISequenceService
    {
        void UpdateActionSequence(Sprite sprite, float deltaTimeTick);
        void IncrementSequence(Sprite sprite, Action action);
    }
}
