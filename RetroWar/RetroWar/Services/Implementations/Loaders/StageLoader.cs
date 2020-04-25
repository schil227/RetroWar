using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Level;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class StageLoader : IStageLoader
    {
        private readonly IStreamReader streamReader;

        public StageLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public IEnumerable<StageDatabaseItem> LoadAllStages(string stageLoaderReferenceJsonFile, string stagesFolder)
        {
            var stageDatabaseItems = new List<StageDatabaseItem>();

            var stagesToLoadJson = streamReader.ReadFile(stageLoaderReferenceJsonFile);

            var stagesToLoad = JsonConvert.DeserializeObject<IEnumerable<string>>(stagesToLoadJson);

            foreach (var stageFile in stagesToLoad)
            {
                var stageJson = streamReader.ReadFile(stagesFolder + stageFile);

                var stage = JsonConvert.DeserializeObject<Stage>(stageJson);

                foreach (var tile in stage.Tiles)
                {
                    stage.TileLookup.Add(new Tuple<int, int>((int)(tile.X / 16), (int)(tile.Y / 16)), tile);
                }

                stageDatabaseItems.Add(new StageDatabaseItem
                {
                    StageId = stage.StageId,
                    Stage = stage
                });
            }

            var duplicateIds = stageDatabaseItems.GroupBy(a => a.StageId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds.Count() > 0)
            {
                throw new StageLoaderException($"Duplicate IDs found when loading Illusions. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            return stageDatabaseItems;
        }
    }
}
