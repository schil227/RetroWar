using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision.Resolvers
{
    public interface ICollisionResolver
    {
        Resolution ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution);
    }
}
