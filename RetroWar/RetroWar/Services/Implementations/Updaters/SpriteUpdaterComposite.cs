using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Updaters;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Updaters
{
    public class SpriteUpdaterComposite : ISpriteUpdater
    {
        private readonly IEnumerable<ISpriteUpdater> spriteUpdaters;

        public SpriteUpdaterComposite(IEnumerable<ISpriteUpdater> spriteUpdaters)
        {
            this.spriteUpdaters = spriteUpdaters;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            foreach (var updater in spriteUpdaters)
            {
                if (!processedSprites.ContainsKey(sprite.SpriteId) && updater.UpdateSprite(sprite, deltaTime, processedSprites))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
