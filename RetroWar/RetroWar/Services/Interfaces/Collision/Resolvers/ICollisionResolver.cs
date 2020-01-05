using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision.Resolvers
{
    public interface ICollisionResolver
    {
        bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution);
    }
}
