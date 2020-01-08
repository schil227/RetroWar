using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories;
using RetroWar.Services.Interfaces.Loaders;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class ContentLoader : IContentLoader
    {
        private readonly IPlayerVehicleLoader playerVehicleLoader;
        private readonly IEnemyVehicleLoader enemyVehicleLoader;
        private readonly IActionDataLoader actionDataLoader;
        private readonly ITextureLoader textureLoader;
        private readonly ITileLoader tileLoader;
        private readonly IBulletLoader bulletLoader;

        public ContentLoader
            (
            IPlayerVehicleLoader playerVehicleLoader,
            IEnemyVehicleLoader enemyVehicleLoader,
            IActionDataLoader actionDataLoader,
            ITextureLoader textureLoader,
            ITileLoader tileLoader,
            IBulletLoader bulletLoader
            )
        {
            this.playerVehicleLoader = playerVehicleLoader;
            this.enemyVehicleLoader = enemyVehicleLoader;
            this.actionDataLoader = actionDataLoader;
            this.textureLoader = textureLoader;
            this.tileLoader = tileLoader;
            this.bulletLoader = bulletLoader;
        }

        ContentDatabase IContentLoader.LoadAllData(
            ContentManager content,
            string playerVehicleFileName,
            string enemyVehicleFileName,
            string actionDataFileName,
            string textureFileName,
            string tileFileName,
            string bulletFileName)
        {
            var contentDatabase = new ContentDatabase();

            contentDatabase.PlayerVehicles = playerVehicleLoader.LoadPlayerVehicles(playerVehicleFileName);
            contentDatabase.EnemyVehicles = enemyVehicleLoader.LoadEnemyVehicles(enemyVehicleFileName);
            contentDatabase.Textures = textureLoader.LoadTextures(textureFileName, content);
            contentDatabase.Tiles = tileLoader.LoadTiles(tileFileName);
            contentDatabase.Bullets = bulletLoader.LoadBullets(bulletFileName);
            contentDatabase.Actions = actionDataLoader.LoadActionData(actionDataFileName);

            foreach (var spriteData in contentDatabase.PlayerVehicles)
            {
                spriteData.Player.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(spriteData.Player.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            foreach (var spriteData in contentDatabase.EnemyVehicles)
            {
                spriteData.Enemy.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(spriteData.Enemy.ActionDataSetId, a.ActionDataId)).ActionData;
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
