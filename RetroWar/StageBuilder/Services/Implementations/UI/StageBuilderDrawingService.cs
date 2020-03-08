using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.UserInterface;
using StageBuilder.Model.UI;
using StageBuilder.Services.Interfaces.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StageBuilder.Services.Implementations.UI
{
    public class StageBuilderDrawingService : IStageBuilderDrawingService
    {
        private static Dictionary<Tuple<int, int>, Texture2D> cachedHitBoxTextures;

        private readonly IDrawService drawService;
        private readonly ISpriteHelper spriteHelper;

        public StageBuilderDrawingService
            (
                IDrawService drawService,
                ISpriteHelper spriteHelper
            )
        {
            this.drawService = drawService;
            this.spriteHelper = spriteHelper;

            cachedHitBoxTextures = new Dictionary<Tuple<int, int>, Texture2D>();
        }

        public void DrawStageBuilderUI(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItems, ConstructionData constructionData)
        {
            DrawGreyWorkTray(spriteBatch, screen);
            DrawTiles(spriteBatch, textureDatabaseItems, screen, constructionData);
        }

        private void DrawGreyWorkTray(SpriteBatch spriteBatch, Screen screen)
        {
            Texture2D greyWorkTrayTexture;

            cachedHitBoxTextures.TryGetValue(new Tuple<int, int>(screen.Width, 24), out greyWorkTrayTexture);

            if (greyWorkTrayTexture == null)
            {
                greyWorkTrayTexture = CreateGreyWorkTryTexture(spriteBatch, screen);
                cachedHitBoxTextures.Add(new Tuple<int, int>(screen.Width, 24), greyWorkTrayTexture);
            }

            spriteBatch.Draw(greyWorkTrayTexture, new Vector2(0, screen.Height - 24), new Color(104, 104, 104));
        }

        private Texture2D CreateGreyWorkTryTexture(SpriteBatch spriteBatch, Screen screen)
        {
            var hitboxRectangle = new Texture2D(spriteBatch.GraphicsDevice, screen.Width, 24);
            Color[] colorData = new Color[screen.Width * 24];

            var colors = Enumerable.Range(0, screen.Width * 24).Select(index => new Color(104, 104, 104)).ToArray();

            // Gold box:
            var startX = (screen.Width / 2) - 10;
            var endX = startX + 10;

            for (int i = 2; i < 22; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var color = new Color(235, 211, 32);

                    colors[startX + (screen.Width * i) + j] = color;
                }
            }

            hitboxRectangle.SetData(colors);

            return hitboxRectangle;
        }

        private void DrawTiles(SpriteBatch spriteBatch, IEnumerable<TextureDatabaseItem> textureDatabaseItems, Screen screen, ConstructionData constructionData)
        {
            var tilesToDraw = new List<Sprite>();

            var startIndex = constructionData.TileIndex - 3;

            while (startIndex < 0)
            {
                startIndex += constructionData.Tiles.Count;
            }

            int stepX = screen.Width / 8;
            int x = stepX - 8;
            int y = screen.Height - 20;

            for (int i = 0; i < 7; i++)
            {
                var tile = constructionData.Tiles.ElementAt((startIndex + i) % constructionData.Tiles.Count);

                tile.X = x;
                tile.Y = y;

                x += stepX;

                foreach (var texture in spriteHelper.GetCurrentTextureData(tile))
                {
                    drawService.DrawSprite(spriteBatch, tile, textureDatabaseItems, screen, texture, true);
                }
            }
        }
    }
}
