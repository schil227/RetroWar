using RetroWar.Models.Repositories.Level;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IStageLoader
    {
        IEnumerable<StageDatabaseItem> LoadAllStages(string stageLoaderReferenceJsonFile, string stagesFolder);
    }
}
