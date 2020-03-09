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
        private readonly IIllusionLoader illusionLoader;
        private readonly IStageLoader stageLoader;

        public ContentLoader
            (
            IPlayerVehicleLoader playerVehicleLoader,
            IEnemyVehicleLoader enemyVehicleLoader,
            IActionDataLoader actionDataLoader,
            ITextureLoader textureLoader,
            ITileLoader tileLoader,
            IBulletLoader bulletLoader,
            IIllusionLoader illusionLoader,
            IStageLoader stageLoader
            )
        {
            this.playerVehicleLoader = playerVehicleLoader;
            this.enemyVehicleLoader = enemyVehicleLoader;
            this.actionDataLoader = actionDataLoader;
            this.textureLoader = textureLoader;
            this.tileLoader = tileLoader;
            this.bulletLoader = bulletLoader;
            this.illusionLoader = illusionLoader;
            this.stageLoader = stageLoader;
        }

        public ContentDatabase LoadAllData(
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
            )
        {
            var contentDatabase = new ContentDatabase();

            contentDatabase.PlayerVehicles = playerVehicleLoader.LoadPlayerVehicles(playerVehicleFileName);
            contentDatabase.EnemyVehicles = enemyVehicleLoader.LoadEnemyVehicles(enemyVehicleFileName);
            contentDatabase.Textures = textureLoader.LoadTextures(textureFileName, content);
            contentDatabase.Tiles = tileLoader.LoadTiles(tileFileName);
            contentDatabase.Bullets = bulletLoader.LoadBullets(bulletFileName);
            contentDatabase.Actions = actionDataLoader.LoadActionData(actionDataFileName);
            contentDatabase.Illusions = illusionLoader.LoadIllusions(illusionFileName);
            contentDatabase.Stages = stageLoader.LoadAllStages(stageFileName, stageFolder);

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

            foreach (var illusionData in contentDatabase.Illusions)
            {
                illusionData.Illusion.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(illusionData.Illusion.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            return contentDatabase;
        }
    }
}
