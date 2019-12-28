using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.UserInterface;
using System;
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

        public void DrawScreen(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItem)
        {
            Sprite playerSprite = null;

            var boxes = gridHandler.GetGridsFromPoints(stage.Grids, screen.X, screen.Y, screen.X + screen.Width, screen.Y + screen.Height);
            var drawnSprites = new Dictionary<string, string>();

            foreach (var box in boxes)
            {
                foreach (var tile in box.TileSprites.Values.ToList())
                {
                    if (drawnSprites.ContainsKey(tile.SpriteId))
                    {
                        continue;
                    }

                    var textures = spriteHelper.GetCurrentTextureData(tile);

                    foreach (var texture in textures)
                    {
                        var position = new Vector2((tile.X + 16 * texture.RelativeX) - screen.X, (tile.Y + 16 * texture.RelativeY) - screen.Y);
                        spriteBatch.Draw(textureDatabaseItem.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture, position, Color.White);
                    }

                    drawnSprites.Add(tile.SpriteId, "drawn");
                }

                if (playerSprite == null && box.PlayerSprite != null)
                {
                    playerSprite = box.PlayerSprite;
                }
            }

            if (playerSprite == null)
            {
                return;
            }

            var flipSpriteHorizontal = playerSprite.CurrentDirection == Direction.Left;

            var playerTextures = spriteHelper.GetCurrentTextureData(playerSprite);

            foreach (var texture in playerTextures)
            {
                var position = new Vector2((playerSprite.X + 16 * texture.RelativeX) - screen.X, (playerSprite.Y + 16 * texture.RelativeY) - screen.Y);
                var textureToDraw = textureDatabaseItem.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture;
                var spriteEffect = flipSpriteHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                var rectangle = new Rectangle(16 * playerSprite.CurrentSequence, 0, 16, 16);

                if (playerSprite.CurrentSequence > 0)
                {
                    Console.WriteLine($"CurrentSequence: {playerSprite.CurrentSequence}");
                }

#pragma warning disable CS0618 // Type or member is obsolete
                spriteBatch.Draw(textureToDraw, position: position, sourceRectangle: rectangle, color: Color.White, effects: spriteEffect);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}
