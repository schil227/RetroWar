using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface ISpriteHelper
    {
        Point GetMaximumPoints(Sprite sprite, int spriteX, int spriteY);
        ActionData GetCurrentActionData(Sprite sprite);
        HitBox[] GetCurrentHitBoxes(Sprite sprite);
        TextureData[] GetCurrentTextureData(Sprite sprite);
    }
}
