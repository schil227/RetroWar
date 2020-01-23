using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Illusions;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class IllusionLoader : IIllusionLoader
    {
        private readonly IStreamReader streamReader;

        public IllusionLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }


        public IEnumerable<IllusionDatabaseItem> LoadIllusions(string illusionFileName)
        {
            var illusionLoaderJson = streamReader.ReadFile(illusionFileName);

            var illusionData = JsonConvert.DeserializeObject<IllusionDatabaseItem[]>(illusionLoaderJson);

            var duplicateIds = illusionData.GroupBy(a => a.IllusionId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds.Count() > 0)
            {
                throw new IllusionLoaderException($"Duplicate IDs found when loading Illusions. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var nonMatchingIds = new List<string>();

            foreach (var item in illusionData)
            {
                if (!string.Equals(item.IllusionId, item.Illusion.SpriteId))
                {
                    nonMatchingIds.Add(item.IllusionId);
                }
            }

            if (nonMatchingIds.Count > 0)
            {
                throw new IllusionLoaderException($"Sprites found with non-matching Ids. Update sprites with the following Ids to be the same: {string.Join(",", nonMatchingIds)}");
            }

            return illusionData;
        }
    }
}
