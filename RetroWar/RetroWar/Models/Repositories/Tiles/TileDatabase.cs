using System.Collections.Generic;

namespace RetroWar.Models.Repositories.Tiles
{
    public class TileDatabase
    {
        public IEnumerable<TileDatabaseItem> TileDatabaseItems { get; set; }
    }
}
