using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.UserInterface
{
    public interface IDrawService
    {
        void DrawScreen(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItem);
    }
}
