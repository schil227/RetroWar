using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IContentLoader
    {
        ContentDatabase LoadAllData(
            ContentManager content,
            string playerVehicleFileName,
            string enemyVehicleFileName,
            string actionDataFileName,
            string textureFileName,
            string tileFileName,
            string bulletFileName,
            string illusionFileName,
            string stageFileName,
            string stageFolder
            );
    }
}
