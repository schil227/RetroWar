using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Models.Sprites.Textures;
using RetroWar.Models.Sprites.Vehicles;
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

        private static Dictionary<Tuple<int, int>, Texture2D> cachedHitBoxTextures;
        private static bool DebugModeEnabled;

        public DrawService(
            IGridHandler gridHandler,
            ISpriteHelper spriteHelper
            )
        {
            this.gridHandler = gridHandler;
            this.spriteHelper = spriteHelper;

            cachedHitBoxTextures = new Dictionary<Tuple<int, int>, Texture2D>();
            DebugModeEnabled = true;
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
                DrawSprites(spriteBatch, textureDatabaseItems, box.Illusions.Values.ToArray(), screen, drawnSprites);

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

        private void DrawSprite(SpriteBatch spriteBatch, Sprite sprite, IEnumerable<TextureDatabaseItem> textureDatabaseItems, Screen screen, KeyValuePair<TextureData, RetroWar.Models.Sprites.Actions.Action> textureActionPair)
        {
            var texture = textureActionPair.Key;
            var action = textureActionPair.Value;

            var relativeX = sprite.CurrentDirection == Direction.Right ? texture.RelativeX : texture.RelativeX * -1;

            var position = new Vector2((sprite.X + 16 * relativeX) - screen.X, (sprite.Y + 16 * texture.RelativeY) - screen.Y);
            var textureToDraw = textureDatabaseItems.First(t => string.Equals(t.TextureId, texture.TextureId)).Texture;
            var spriteEffect = sprite.CurrentDirection == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var rectangle = new Rectangle(16 * sprite.CurrentActionSequence[action], 0, 16, 16);

#pragma warning disable CS0618 // Type or member is obsolete
            spriteBatch.Draw(textureToDraw, position: position, sourceRectangle: rectangle, color: Color.White, effects: spriteEffect);
#pragma warning restore CS0618 // Type or member is obsolete

            if (DebugModeEnabled)
            {
                DrawHitbox(sprite, spriteBatch, screen);
            }
        }

        private void DrawHitbox(Sprite sprite, SpriteBatch spriteBatch, Screen screen)
        {
            var currentHitBox = spriteHelper.GetCurrentHitBoxes(sprite).FirstOrDefault();

            if (currentHitBox == null)
            {
                return;
            }

            Texture2D hitboxRectangle;
            Vector2 position;

            cachedHitBoxTextures.TryGetValue(new Tuple<int, int>(currentHitBox.Width, currentHitBox.Height), out hitboxRectangle);

            if (hitboxRectangle == null)
            {
                hitboxRectangle = MakeHitBoxTexture(spriteBatch, sprite, currentHitBox);
                cachedHitBoxTextures.Add(new Tuple<int, int>(currentHitBox.Width, currentHitBox.Height), hitboxRectangle);
            }

            position.X = sprite.X + spriteHelper.GetHitboxXOffset(sprite, currentHitBox.RelativeX, currentHitBox.Width) - screen.X;
            position.Y = sprite.Y + currentHitBox.RelativeY - screen.Y;

            spriteBatch.Draw(hitboxRectangle, position, Color.Red);
        }

        private Texture2D MakeHitBoxTexture(SpriteBatch spriteBatch, Sprite sprite, HitBox currentHitBox)
        {
            var hitboxRectangle = new Texture2D(spriteBatch.GraphicsDevice, currentHitBox.Width, currentHitBox.Height);
            Color[] colorData = new Color[currentHitBox.Width * currentHitBox.Height];

            for (int i = 0; i < currentHitBox.Width; i++)
            {
                for (int j = 0; j < currentHitBox.Height; j++)
                {
                    // make the a red-outlined box with a transparent middle for the hitboxes hitboxes
                    if (i == 0 || i == currentHitBox.Width - 1 || j == 0 || j == currentHitBox.Height - 1)
                    {
                        colorData[i + j * currentHitBox.Width] = Color.Red;
                    }
                    else
                    {
                        colorData[i + j * currentHitBox.Width] = Color.Transparent;
                    }

                    hitboxRectangle.SetData(colorData);
                }
            }

            return hitboxRectangle;
        }
    }
}
