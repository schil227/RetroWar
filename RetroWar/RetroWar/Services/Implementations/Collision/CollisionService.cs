using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionService : ICollisionService
    {
        private readonly ISpriteHelper spriteHelper;
        private readonly IFoundCollisionFilter foundCollisionFilter;
        private readonly ICollisionResolver collisionResolver;

        public CollisionService
            (
                ISpriteHelper spriteHelper,
                IFoundCollisionFilter foundCollisionFilter,
                ICollisionResolver collisionResolver
            )
        {
            this.spriteHelper = spriteHelper;
            this.foundCollisionFilter = foundCollisionFilter;
            this.collisionResolver = collisionResolver;
        }

        public CollisionResolution[] GetCollisions(Sprite normal, Sprite based)
        {
            var normalHitBoxes = spriteHelper.GetCurrentHitBoxes(normal);
            var basedHitBoxes = spriteHelper.GetCurrentHitBoxes(based);

            var collisionResolutions = new List<CollisionResolution>();

            foreach (var normalBox in normalHitBoxes)
            {
                foreach (var basedBox in basedHitBoxes)
                {
                    var filteredCollision = foundCollisionFilter.FilterCollisionsFound(normal, based, normalBox, basedBox);

                    if (filteredCollision != null)
                    {
                        collisionResolutions.Add(filteredCollision);
                    }
                }
            }

            return collisionResolutions.ToArray();
        }

        public Resolution ResolveCollision(Sprite normal, Sprite based, CollisionResolution[] collisions, bool withRespectToX)
        {
            if (collisions.Length == 0)
            {
                return Resolution.None;
            }

            var bestCollisionResolution = foundCollisionFilter.FindBestCollision(collisions);
            return collisionResolver.ResolveCollision(normal, based, bestCollisionResolution, withRespectToX);
        }
    }
}
