using RetroWar.Models.Repositories.Tiles;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface ITileLoader
    {
        IEnumerable<TileDatabaseItem> LoadTiles(string TileFileName);
    }
}
