using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Linq;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class SpriteHelper : ISpriteHelper
    {
        public HitBox[] GetCurrentHitBoxes(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                        .ActionHitBoxSet.ElementAt(sprite.CurrentSequence).HitBoxes.ToArray();
        }

        public TextureData[] GetCurrentTextureData(Sprite sprite)
        {
            return sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction)
                            .ActionTextureSet.ElementAt(sprite.CurrentSequence).TextureData.ToArray();
        }
    }
}
