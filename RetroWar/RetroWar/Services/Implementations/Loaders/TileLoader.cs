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

            return tileData;
        }
    }
}
