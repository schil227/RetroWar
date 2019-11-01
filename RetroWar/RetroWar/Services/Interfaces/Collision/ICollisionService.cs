using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionService
    {
        CollisionResolution[] GetCollisions(Sprite normal, Sprite based);
        Resolution ResolveCollision(Sprite normal, Sprite based, CollisionResolution[] collisions);
    }
}
