using RetroWar.Models.Common;
using RetroWar.Models.Sprites;

namespace RetroWar.Models.Events
{
    public class SpawnSpriteEvent : EventBase
    {
        public string SpriteId { get; set; }
        public Point SpriteSpawnLocation { get; set; }
        public SpriteType SpriteType { get; set; }
    }
}
