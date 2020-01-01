using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Tiles;
using System.Collections.Generic;

namespace RetroWar.Models.Collisions.Grid
{
    public class GridContainer
    {
        public Sprite PlayerSprite { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; }
        public Dictionary<string, Bullet> Bullets { get; set; }
    }
}
