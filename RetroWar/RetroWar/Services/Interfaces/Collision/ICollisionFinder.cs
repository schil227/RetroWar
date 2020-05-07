using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionFinder
    {
        CollisionResolution FindCollisionResolutionFace(Sprite normal, Sprite based, float deltaTime);
        bool IsOnTopOf(Vehicle normal, Vehicle based);
        bool IsRightOf(Vehicle normal, Vehicle based);
    }
}
