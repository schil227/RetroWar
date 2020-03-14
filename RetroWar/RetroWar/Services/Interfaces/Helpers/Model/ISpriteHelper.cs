using Microsoft.Xna.Framework;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface ISpriteHelper
    {
        Point GetMaximumPoints(Sprite sprite, int spriteX, int spriteY);
        IEnumerable<ActionData> GetCurrentActionData(Sprite sprite);
        HitBox GetHitBox(Sprite sprite);
        IDictionary<TextureData, Action> GetCurrentTextureData(Sprite sprite);
        int GetHitboxXOffset(Sprite sprite, int currentXOffset, int hitBoxWidth);
    }
}
