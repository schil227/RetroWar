using RetroWar.Models.Level;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Models.Sprites.Tiles;
using System.Collections.Generic;

namespace StageBuilder.Model.UI
{
    public class ConstructionData
    {
        public Stage Stage { get; set; }
        public Illusion Cursor { get; set; }
        public int TileIndex { get; set; }
        public List<Tile> Tiles { get; set; }

    }
}
