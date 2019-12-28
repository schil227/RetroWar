using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories.Textures;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface ITextureLoader
    {
        IEnumerable<TextureDatabaseItem> LoadTextures(string texturesFileName, ContentManager Content);
    }
}
