using Newtonsoft.Json;

namespace RetroWar.Models.Sprites.Textures
{
    public class TextureData
    {
        [JsonProperty]
        public string TextureId { get; private set; }
        [JsonProperty]
        public int RelativeX { get; private set; }
        [JsonProperty]
        public int RelativeY { get; private set; }
    }
}
