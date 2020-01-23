using RetroWar.Models.Repositories.Illusions;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IIllusionLoader
    {
        IEnumerable<IllusionDatabaseItem> LoadIllusions(string illusionFileName);
    }
}
