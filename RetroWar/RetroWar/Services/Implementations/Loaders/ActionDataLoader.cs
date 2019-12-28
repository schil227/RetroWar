using Newtonsoft.Json;
using RetroWar.Exceptions.Implementations.Loaders;
using RetroWar.Models.Repositories.Actions;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;
using System.Collections.Generic;
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

        public IEnumerable<ActionDataDatabaseItem> LoadActionData(string actionDataJsonFileName)
        {
            var actionDataJson = streamReader.ReadFile(actionDataJsonFileName);

            var actionData = JsonConvert.DeserializeObject<IEnumerable<ActionDataDatabaseItem>>(actionDataJson);

            var duplicateIds = actionData.GroupBy(a => a.ActionDataId).Where(g => g.Count() > 1)?.Select(i => i.Key);

            if (duplicateIds?.Count() > 0)
            {
                throw new ActionDataLoaderException($"Duplicate IDs found when loading AcitonData. Ids: {string.Join(",", duplicateIds.Distinct())}");
            }

            var incorrectSequenceDataIDs = new HashSet<string>();

            foreach (var data in actionData)
            {
                foreach (var action in data.ActionData)
                {
                    if (action.TotalSequences != action.SequenceDurations.Count())
                    {
                        incorrectSequenceDataIDs.Add($"Id: {data.ActionDataId}, Action: {action.Action}");
                    }
                }
            }

            if (incorrectSequenceDataIDs.Count > 0)
            {
                throw new ActionDataLoaderException($"Found inconsistant sequences: the TotalSequences for an action must have an entry in SequenceDurations for each frame. Check the following entries: {string.Join(", ", incorrectSequenceDataIDs)}");
            }

            var incorrectEventsData = new HashSet<string>();

            foreach (var data in actionData)
            {
                foreach (var action in data.ActionData)
                {
                    if (action.TotalSequences != action.Events.Count())
                    {
                        incorrectSequenceDataIDs.Add($"Id: {data.ActionDataId}, Action: {action.Action}");
                    }
                }
            }

            if (incorrectSequenceDataIDs.Count > 0)
            {
                throw new ActionDataLoaderException($"Found inconsistant events: the TotalSequences for an action must have an entry in events for each frame. Check the following entries: {string.Join(", ", incorrectSequenceDataIDs)}");
            }

            return actionData;
        }
    }
}
