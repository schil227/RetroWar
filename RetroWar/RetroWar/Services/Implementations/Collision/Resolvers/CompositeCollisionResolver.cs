using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class CompositeCollisionResolver : ICollisionResolver
    {
        private readonly IEnumerable<ICollisionResolver> collisionResolvers;

        public CompositeCollisionResolver
            (
            IEnumerable<ICollisionResolver> collisionResolvers
            )
        {
            this.collisionResolvers = collisionResolvers;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            foreach (var resolver in collisionResolvers)
            {
                if (resolver.ResolveCollision(normal, based, collisionResolution))
                {
                    return true;
                }
            }

            Console.WriteLine($"Could not find collision resolver between sprites. Normal Id: {normal.SpriteId}, Base Id: {based.SpriteId}");
            return false;
        }
    }
}
