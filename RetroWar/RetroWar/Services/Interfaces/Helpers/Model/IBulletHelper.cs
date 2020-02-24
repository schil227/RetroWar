using RetroWar.Models.Common;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Vehicles;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface IBulletHelper
    {
        Point FindNextPointInTrajectory(Bullet bullet, float deltaTime);
        void FireBullet(Vehicle vehicle, FiringMode firingMode);
    }
}
