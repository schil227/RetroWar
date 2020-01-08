using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Vehicles;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    internal class EnemyVehicleLoader : IEnemyVehicleLoader
    {
        private readonly IStreamReader streamReader;

        public EnemyVehicleLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public EnemyVehicleDatabaseItem[] LoadEnemyVehicles(string enemyVehicleFileName)
        {
            var vehicleLoaderJson = streamReader.ReadFile(enemyVehicleFileName);

            var vehicleData = JsonConvert.DeserializeObject<EnemyVehicleDatabaseItem[]>(vehicleLoaderJson);

            var duplicateIds = vehicleData.GroupBy(a => a.EnemyId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new SpriteLoaderException($"Duplicate IDs found when loading Enemy Vehicles. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in vehicleData)
            {
                if (!string.Equals(item.EnemyId, item.Enemy.SpriteId))
                {
                    nonMatchingIds.Add(item.EnemyId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new SpriteLoaderException($"Vehicles found with non-matching Ids. Update enemy vehicles with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return vehicleData;
        }
    }
}
