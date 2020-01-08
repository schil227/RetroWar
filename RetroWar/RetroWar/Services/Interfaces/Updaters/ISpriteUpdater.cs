using RetroWar.Models.Sprites;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Updaters
{
    public interface ISpriteUpdater
    {
        bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites);
    }
}
