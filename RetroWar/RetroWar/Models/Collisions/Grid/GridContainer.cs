using RetroWar.Models.Sprites;
using System.Collections.Generic;

namespace RetroWar.Models.Collisions.Grid
{
    public class GridContainer
    {
        public Sprite PlayerSprite { get; set; }
        public Dictionary<string, Sprite> TileSprites { get; set; }
    }
}
