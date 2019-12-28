using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories;
using RetroWar.Services.Interfaces.Loaders;
using System.Linq;

namespace RetroWar.Services.Implementations.Loaders
{
    public class ContentLoader : IContentLoader
    {
        private readonly ISpriteLoader spriteLoader;
        private readonly IActionDataLoader actionDataLoader;
        private readonly ITextureLoader textureLoader;
        private readonly ITileLoader tileLoader;

        public ContentLoader
            (
            ISpriteLoader spriteLoader,
            IActionDataLoader actionDataLoader,
            ITextureLoader textureLoader,
            ITileLoader tileLoader
            )
        {
            this.spriteLoader = spriteLoader;
            this.actionDataLoader = actionDataLoader;
            this.textureLoader = textureLoader;
            this.tileLoader = tileLoader;
        }

        ContentDatabase IContentLoader.LoadAllData(ContentManager Content, string spriteFileName, string actionDataFileName, string textureFileName, string tileFileName)
        {
            var contentDatabase = new ContentDatabase();

            contentDatabase.Sprites = spriteLoader.LoadSprites(spriteFileName);
            contentDatabase.Textures = textureLoader.LoadTextures(textureFileName, Content);
            contentDatabase.Tiles = tileLoader.LoadTiles(tileFileName);
            contentDatabase.Actions = actionDataLoader.LoadActionData(actionDataFileName);

            foreach (var spriteData in contentDatabase.Sprites)
            {
                spriteData.Sprite.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(spriteData.Sprite.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            foreach (var tileData in contentDatabase.Tiles)
            {
                tileData.Tile.ActionDataSet = contentDatabase.Actions.First(a => string.Equals(tileData.Tile.ActionDataSetId, a.ActionDataId)).ActionData;
            }

            return contentDatabase;
        }
    }
}
