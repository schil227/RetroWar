using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites.Textures
{
    public class TextureDataCollection
    {
        [JsonProperty]
        public IEnumerable<TextureData> TextureData { get; private set; }
    }
}
