using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Bullets;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class BulletLoader : IBulletLoader
    {
        private readonly IStreamReader streamReader;

        public BulletLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public IEnumerable<BulletDatabaseItem> LoadBullets(string bulletFileName)
        {
            var bulletLoaderJson = streamReader.ReadFile(bulletFileName);

            var bulletData = JsonConvert.DeserializeObject<BulletDatabaseItem[]>(bulletLoaderJson);

            var duplicateIds = bulletData.GroupBy(a => a.BulletId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds.Count() > 0)
            {
                throw new BulletLoaderException($"Duplicate IDs found when loading Bullets. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in bulletData)
            {
                if (!string.Equals(item.BulletId, item.Bullet.SpriteId))
                {
                    nonMatchingIds.Add(item.BulletId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new BulletLoaderException($"Sprites found with non-matching Ids. Update sprites with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return bulletData;
        }
    }
}
