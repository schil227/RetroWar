using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;

namespace RetroWar.Services.Interfaces.Factories
{
    public interface ISpriteFactory
    {
        Bullet CreateBullet(string bulletId, Point point, Direction direction);
    }
}
