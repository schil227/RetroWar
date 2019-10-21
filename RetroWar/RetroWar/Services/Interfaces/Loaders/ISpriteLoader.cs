using RetroWar.Models.Repositories.Sprites;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface ISpriteLoader
    {
        SpriteDatabaseItem[] LoadSprites(string SpriteFileName);
    }
}
