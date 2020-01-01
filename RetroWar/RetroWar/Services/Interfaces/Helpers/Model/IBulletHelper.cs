using RetroWar.Models.Common;
using RetroWar.Models.Sprites.Bullets;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface IBulletHelper
    {
        Point FindNextPointInTrajectory(Bullet bullet, float deltaTime);
    }
}
