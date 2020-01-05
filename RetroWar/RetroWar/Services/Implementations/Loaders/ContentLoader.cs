using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories;
using RetroWar.Services.Interfaces.Loaders;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class ContentLoader : IContentLoader
    {
        private readonly IVehicleLoader vehicleLoader;
        private readonly IActionDataLoader actionDataLoader;
        private readonly ITextureLoader textureLoader;
        private readonly ITileLoader tileLoader;
        private readonly IBulletLoader bulletLoader;

        public ContentLoader
            (
            IVehicleLoader vehicleLoader,
            IActionDataLoader actionDataLoader,
            ITextureLoader textureLoader,
            ITileLoader tileLoader,
            IBulletLoader bulletLoader
            )
        {
            this.vehicleLoader = vehicleLoader;
            this.actionDataLoader = actionDataLoader;
            this.textureLoader = textureLoader;
            this.tileLoader = tileLoader;
            this.bulletLoader = bulletLoader;
        }

        ContentDatabase IContentLoader.LoadAllData(ContentManager content, string vehicleFileName, string actionDataFileName, string textureFileName, string tileFileName, string bulletFileName)
        {
            var contentDatabase = new ContentDatabase();

            contentDatabase.Vehicles = vehicleLoader.LoadVehicles(vehicleFileName);
            contentDatabase.Textures = textureLoader.LoadTextures(textureFileName, content);
            contentDatabase.Tiles = tileLoader.LoadTiles(tileFileName);
            contentDatabase.Bullets = bulletLoader.LoadBullets(bulletFileName);
            contentDatabase.Actions = actionDataLoader.LoadActionData(actionDataFileName);

            foreach (var spriteData in contentDatabase.Vehicles)
            {
                spriteData.Vehicle.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(spriteData.Vehicle.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            foreach (var tileData in contentDatabase.Tiles)
            {
                tileData.Tile.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(tileData.Tile.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            foreach (var bulletData in contentDatabase.Bullets)
            {
                bulletData.Bullet.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(bulletData.Bullet.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            return contentDatabase;
        }
    }
}
