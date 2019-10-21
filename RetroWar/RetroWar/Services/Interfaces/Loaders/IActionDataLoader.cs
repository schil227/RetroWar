using RetroWar.Models.Repositories;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IActionDataLoader
    {
        ActionDataDatabaseItem[] LoadActionData(string actionDataJsonFileName);
    }
}
