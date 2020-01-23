using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Illusions;

namespace RetroWar.Services.Interfaces.Factories
{
    public interface ISpriteFactory
    {
        Bullet CreateBullet(string bulletId, Point point, Direction direction, DamageDiscrimination damageDiscrimination);
        Illusion CreateIllusion(string illusionId, Point point);
    }
}
