﻿using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Resolvers;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionService : ICollisionService
    {
        private readonly ICollisionResolver collisionResolver;
        private readonly ICollisionChecker collisionChecker;
        private readonly ICollisionFinder collisionFinder;

        public CollisionService
            (
                ICollisionResolver collisionResolver,
                ICollisionChecker collisionChecker,
                ICollisionFinder collisionFinder
            )
        {
            this.collisionResolver = collisionResolver;
            this.collisionChecker = collisionChecker;
            this.collisionFinder = collisionFinder;
        }

        public bool HandleCollision(Sprite normal, Sprite based, float deltaTime)
        {
            if (!collisionChecker.AreColliding(normal, based))
            {
                return true;
            }

            var collision = collisionFinder.FindCollisionResolutionFace(normal, based, deltaTime);

            return collisionResolver.ResolveCollision(normal, based, collision);
        }
    }
}
