using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Textures;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.UserInterface
{
    public interface IDrawService
    {
        void DrawScreen(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItem);
        void DrawSprite(SpriteBatch spriteBatch, Sprite sprite, IEnumerable<TextureDatabaseItem> textureDatabaseItems, Screen screen, KeyValuePair<TextureData, RetroWar.Models.Sprites.Actions.Action> textureActionPair, bool screenLocked);
    }
}
