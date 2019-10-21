using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class ActionData
    {
        readonly public Action Action;
        readonly public int TotalSequences;
        readonly public IEnumerable<TextureDataCollection> ActionTextureSet;
        readonly public IEnumerable<HitBoxCollection> ActionHitBoxSet;
    }
}
