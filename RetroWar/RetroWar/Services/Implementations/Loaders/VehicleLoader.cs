using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Vehicles;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    internal class VehicleLoader : IVehicleLoader
    {
        private readonly IStreamReader streamReader;

        public VehicleLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public VehicleDatabaseItem[] LoadVehicles(string vehicleFileName)
        {
            var vehicleLoaderJson = streamReader.ReadFile(vehicleFileName);

            var vehicleData = JsonConvert.DeserializeObject<VehicleDatabaseItem[]>(vehicleLoaderJson);

            var duplicateIds = vehicleData.GroupBy(a => a.VehicleId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new SpriteLoaderException($"Duplicate IDs found when loading Vehicles. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in vehicleData)
            {
                if (!string.Equals(item.VehicleId, item.Vehicle.SpriteId))
                {
                    nonMatchingIds.Add(item.VehicleId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new SpriteLoaderException($"Vehicles found with non-matching Ids. Update vehicles with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return vehicleData;
        }
    }
}
