using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Actions
{
    public interface ISequenceService
    {
        void UpdateActionSequence(Sprite sprite, float deltaTimeTick);
        void IncrementSequence(Sprite sprite);
    }
}
