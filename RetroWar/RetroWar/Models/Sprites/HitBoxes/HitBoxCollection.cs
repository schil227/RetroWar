using Newtonsoft.Json;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites.HitBoxes
{
    public class HitBoxCollection
    {
        [JsonProperty]
        public IEnumerable<HitBox> HitBoxes { get; private set; }
    }
}
