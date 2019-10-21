using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    internal class ActionDataLoader : IActionDataLoader
    {
        private readonly IStreamReader streamReader;

        public ActionDataLoader(IStreamReader streamReader)
        {
            this.streamReader = streamReader;
        }

        public ActionDataDatabaseItem[] LoadActionData(string actionDataJsonFileName)
        {
            var actionDataJson = streamReader.ReadFile(actionDataJsonFileName);

            var actionData = JsonConvert.DeserializeObject<ActionDataDatabaseItem[]>(actionDataJson);

            var duplicateIds = actionData.GroupBy(a => a.ActionDataId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new ActionDataLoaderException($"Duplicate IDs found when loading AcitonData. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            return actionData;
        }
    }
}
