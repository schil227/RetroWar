using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Updaters;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Updaters
{
    public class SpriteUpdaterComposite : ISpriteUpdater
    {
        private readonly IEnumerable<ISpriteUpdater> spriteUpdaters;
        private readonly ISequenceService sequenceService;

        public SpriteUpdaterComposite(
            IEnumerable<ISpriteUpdater> spriteUpdaters,
            ISequenceService sequenceService
            )
        {
            this.spriteUpdaters = spriteUpdaters;
            this.sequenceService = sequenceService;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            foreach (var updater in spriteUpdaters)
            {
                if (!processedSprites.ContainsKey(sprite.SpriteId) && updater.UpdateSprite(sprite, deltaTime, processedSprites))
                {
                    sequenceService.UpdateActionSequence(sprite, deltaTime * 1000);
                    return true;
                }
            }

            return false;
        }
    }
}
