using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionService
    {
        CollisionResolution GetCollision(Sprite normal, Sprite based);
        bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisions);
    }
}
