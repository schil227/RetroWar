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

        public CollisionResolution GetCollision(Sprite normal, Sprite based)
        {
            var normalHitBox = spriteHelper.GetHitBox(normal);
            var basedHitBox = spriteHelper.GetHitBox(based);

            var collisionResolutions = new List<CollisionResolution>();

            if (normalHitBox == null || basedHitBox == null)
            {
                return null;
            }

            return foundCollisionFilter.FilterCollisionsFound(normal, based, normalHitBox, basedHitBox);
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collision)
        {
            if (collision == null)
            {
                return false;
            }

            return collisionResolver.ResolveCollision(normal, based, collision);
        }
    }
}
