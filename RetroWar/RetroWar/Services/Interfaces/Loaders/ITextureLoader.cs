using Microsoft.Xna.Framework.Content;
using RetroWar.Models.Repositories.Textures;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface ITextureLoader
    {
        TextureDatabaseItem[] LoadTextures(string TexturesFileName, ContentManager Content);
    }
}
