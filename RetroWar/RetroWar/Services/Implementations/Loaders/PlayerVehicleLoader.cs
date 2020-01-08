using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Vehicles;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    internal class PlayerVehicleLoader : IPlayerVehicleLoader
    {
        private readonly IStreamReader streamReader;

        public PlayerVehicleLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public PlayerVehicleDatabaseItem[] LoadPlayerVehicles(string playerVehicleFileName)
        {
            var vehicleLoaderJson = streamReader.ReadFile(playerVehicleFileName);

            var vehicleData = JsonConvert.DeserializeObject<PlayerVehicleDatabaseItem[]>(vehicleLoaderJson);

            var duplicateIds = vehicleData.GroupBy(a => a.PlayerId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new SpriteLoaderException($"Duplicate IDs found when loading Player Vehicles. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in vehicleData)
            {
                if (!string.Equals(item.PlayerId, item.Player.SpriteId))
                {
                    nonMatchingIds.Add(item.PlayerId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new SpriteLoaderException($"Vehicles found with non-matching Ids. Update player vehicles with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return vehicleData;
        }
    }
}
