using RetroWar.Models.Sprites;
using System.Collections.Generic;

namespace RetroWar.Models.Repositories
{
    public class ActionDataDatabaseItem
    {
        readonly public string ActionDataId;
        readonly public IEnumerable<ActionData> ActionData;
    }
}
