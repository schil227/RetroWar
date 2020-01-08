using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.UserInterface;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.UserInterface
{
    public class DrawService : IDrawService
    {
        private readonly IGridHandler gridHandler;
        private readonly ISpriteHelper spriteHelper;

        public DrawService(
            IGridHandler gridHandler,
            ISpriteHelper spriteHelper
            )
        {
            this.gridHandler = gridHandler;
            this.spriteHelper = spriteHelper;
        }

        public void DrawScreen(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItems)
        {
            Vehicle playerTank = null;

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);
            var drawnSprites = new Dictionary<string, string>();

            foreach (var box in boxes)
            {
                DrawSprites(spriteBatch, textureDatabaseItems, box.Tiles.Values.ToArray(), screen, drawnSprites);
                DrawSprites(spriteBatch, textureDatabaseItems, box.Bullets.Values.ToArray(), screen, drawnSprites);
                DrawSprites(spriteBatch, textureDatabaseItems, box.EnemyVehicles.Values.ToArray(), screen, drawnSprites);

                if (playerTank == null && box.playerTank != null)
                {
                    playerTank = box.playerTank;
                }
            }

            if (playerTank == null)
            {
                return;
            }

            var flipSpriteHorizontal = playerTank.CurrentDirection == Direction.Left;

            var playerTextures = spriteHelper.GetCurrentTextureData(playerTank);

            foreach (var texture in playerTextures)
            {
                DrawSprite(spriteBatch, playerTank, textureDatabaseItems, screen, texture);
            }
        }

        private void DrawSprites(SpriteBatch spriteBatch, IEnumerable<TextureDatabaseItem> textureDatabaseItems, IEnumerable<Sprite> sprites, Screen screen, Dictionary<string, string> drawnSprites)
        {
            foreach (var sprite in sprites)
            {
                if (drawnSprites.ContainsKey(sprite.SpriteId))
                {
                    continue;
                }

                var textures = spriteHelper.GetCurrentTextureData(sprite);

                foreach (var texture in textures)
                {
                    DrawSprite(spriteBatch, sprite, textureDatabaseItems, screen, texture);
                }

                drawnSprites.Add(sprite.SpriteId, "drawn");
            }
        }

        private void DrawSprite(SpriteBatch spriteBatch, Sprite sprite, IEnumerable<TextureDatabaseItem> textureDatabaseItems, Screen screen, TextureData texture)
        {
            var position = new Vector2((sprite.X + 16 * texture.RelativeX) - screen.X, (sprite.Y + 16 * texture.RelativeY) - screen.Y);
            var textureToDraw = textureDatabaseItems.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture;
            var spriteEffect = sprite.CurrentDirection == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var rectangle = new Rectangle(16 * sprite.CurrentSequence, 0, 16, 16);

#pragma warning disable CS0618 // Type or member is obsolete
            spriteBatch.Draw(textureToDraw, position: position, sourceRectangle: rectangle, color: Color.White, effects: spriteEffect);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
