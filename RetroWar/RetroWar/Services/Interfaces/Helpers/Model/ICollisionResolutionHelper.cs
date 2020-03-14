using RetroWar.Models.Collisions;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface ICollisionResolutionHelper
    {
        void InvertCollisionResolution(CollisionResolution collisionResolution);
    }
}
