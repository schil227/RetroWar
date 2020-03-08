using Microsoft.Xna.Framework.Graphics;
using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Screen;
using StageBuilder.Model.UI;
using System.Collections.Generic;

namespace StageBuilder.Services.Interfaces.UI
{
    public interface IStageBuilderDrawingService
    {
        void DrawStageBuilderUI(SpriteBatch spriteBatch, Stage stage, Screen screen, IEnumerable<TextureDatabaseItem> textureDatabaseItem, ConstructionData constructionData);
    }
}
