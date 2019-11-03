using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Tiles;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class TileLoader : ITileLoader
    {
        private readonly IStreamReader streamReader;

        public TileLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public IEnumerable<TileDatabaseItem> LoadTiles(string tileFileName)
        {
            var tileLoaderJson = streamReader.ReadFile(tileFileName);

            var tileData = JsonConvert.DeserializeObject<TileDatabaseItem[]>(tileLoaderJson);

            var duplicateIds = tileData.GroupBy(a => a.TileId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds.Count() > 0)
            {
                throw new TileLoaderException($"Duplicate IDs found when loading Tiles. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in tileData)
            {
                if (!string.Equals(item.TileId, item.Tile.SpriteId))
                {
                    nonMatchingIds.Add(item.TileId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new TileLoaderException($"Sprites found with non-matching Ids. Update sprites with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return tileData;
        }
    }
}
