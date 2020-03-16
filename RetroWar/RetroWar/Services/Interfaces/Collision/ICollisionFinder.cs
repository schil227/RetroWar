using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionFinder
    {
        CollisionResolution FindCollisionResolutionFace(Sprite normal, Sprite based, float deltaTime);
    }
}
