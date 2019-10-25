using RetroWar.Exceptions.Implementations.Collision;
using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Services.Interfaces.Collision;
using System.Linq;

namespace RetroWar.Services.Implementations.Collision
{
    public class FoundCollisionFilter : IFoundCollisionFilter
    {
        private readonly ICollisionFinder collisionFinder;
        private readonly IMultiPointCollisionResolver multiPointCollisionResolver;

        public FoundCollisionFilter(ICollisionFinder collisionFinder)
        {
            this.collisionFinder = collisionFinder;
        }

        public CollisionResolution FilterCollisionsFound(Sprite normal, Sprite based, HitBox normalBox, HitBox basedBox)
        {
            // if there are none, check for base sprite
            // if two, 
            // if four,
            // if one, return
            // if none, return null
            // throw exception (basically, for 3)

            var collisions = collisionFinder.FindCollisions(normal, based, normalBox, basedBox);

            if (collisions.Count == 0)
            {
                var collisionsInBase = collisionFinder.FindCollisions(based, normal, basedBox, normalBox);

                if (collisions.Count == 0)
                {
                    return null;
                }

                /*
                * B is partially contained in N
                *      ┌────┐
                *      | N *┼───┐
                *      |   || B |
                *      |   *┼───┘
                *      └────┘
                */

                if (collisions.Count == 2)
                {
                    return NegateResolutionDeltas(
                            multiPointCollisionResolver.ResolveTwoPointCollision(collisions)
                        );
                }

                /*
                * B is contained in N
                *      ┌──────────┐
                *      | N *────* |
                *      |   |  B | |
                *      |   *────* |
                *      └──────────┘
                */
                if (collisions.Count == 4)
                {
                    var result = NegateResolutionDeltas(
                            multiPointCollisionResolver.ResolveFourPointCollision(collisions)
                        );
                }

                return null;
            }

            if (collisions.Count == 1)
            {
                return collisions.First();
            }

            /*
            * N is partially contained in B
            *      ┌────┐
            *      | B *┼───┐
            *      |   || N |
            *      |   *┼───┘
            *      └────┘
            */

            if (collisions.Count == 2)
            {
                return multiPointCollisionResolver.ResolveTwoPointCollision(collisions);
            }

            /*
            * N is contained in B
            *      ┌──────────┐
            *      | B *────* |
            *      |   |  N | |
            *      |   *────* |
            *      └──────────┘
            */
            if (collisions.Count == 4)
            {
                return multiPointCollisionResolver.ResolveFourPointCollision(collisions);
            }

            throw new FoundCollisionFilterException($"Illegal number of collisions found. Number of collisions: {collisions.Count}");
        }

        public CollisionResolution FindBestCollision(CollisionResolution[] collisions)
        {
            // perfect.
            if (collisions.Length == 0)
            {
                return null;
            }

            return collisions.First();
        }

        private CollisionResolution NegateResolutionDeltas(CollisionResolution collision)
        {
            collision.DeltaX = collision.DeltaX * -1;
            collision.DeltaY = collision.DeltaY * -1;
            return collision;
        }
    }
}
