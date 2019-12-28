using RetroWar.Models.Sprites.Actions;
using System.Collections.Generic;

namespace RetroWar.Models.Repositories.Actions
{
    public class ActionDataDatabaseItem
    {
        public string ActionDataId;
        public IEnumerable<ActionData> ActionData;
    }
}
