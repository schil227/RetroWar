using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface ISpriteHelper
    {
        HitBox[] GetCurrentHitBoxes(Sprite sprite);
        TextureData[] GetCurrentTextureData(Sprite sprite);
    }
}
