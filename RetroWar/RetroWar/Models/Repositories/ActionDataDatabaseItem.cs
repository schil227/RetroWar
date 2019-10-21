using RetroWar.Models.Sprites;
using System.Collections.Generic;

namespace RetroWar.Models.Repositories
{
    public class ActionDataDatabaseItem
    {
        public string ActionDataId;
        public IEnumerable<ActionData> ActionData;
    }
}
