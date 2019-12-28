using RetroWar.Models.Repositories.Actions;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IActionDataLoader
    {
        IEnumerable<ActionDataDatabaseItem> LoadActionData(string actionDataJsonFileName);
    }
}
