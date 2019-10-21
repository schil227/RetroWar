using Newtonsoft.Json;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class ActionData
    {
        [JsonProperty]
        public Action Action { get; private set; }

        [JsonProperty]
        public int TotalSequences { get; private set; }

        [JsonProperty]
        public IEnumerable<TextureDataCollection> ActionTextureSet { get; private set; }

        [JsonProperty]
        public IEnumerable<HitBoxCollection> ActionHitBoxSet { get; private set; }
    }
}
