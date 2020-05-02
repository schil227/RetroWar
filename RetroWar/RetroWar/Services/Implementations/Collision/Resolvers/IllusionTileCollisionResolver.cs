using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Collision;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class IllusionTileCollisionResolver : ICollisionResolver
    {
        private readonly IResolverHelper resolverHelper;

        public IllusionTileCollisionResolver
            (
                IResolverHelper resolverHelper
            )
        {
            this.resolverHelper = resolverHelper;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Illusion illusion = null;
            Tile tile = null;

            if (normal is Illusion && based is Tile)
            {
                illusion = (Illusion)normal;
                tile = (Tile)based;
            }
            else if (normal is Tile && based is Illusion)
            {
                illusion = (Illusion)based;
                tile = (Tile)normal;
            }
            else
            {
                return false;
            }

            if (!illusion.SubjectToGravity)
            {
                return true;
            }

            var beforeY = illusion.Y;

            // vehicle is "stuck" in tile, nothing to do.
            if (collisionResolution.PrimaryFace == null && collisionResolution.SecondaryFace == null)
            {
                return true;
            }

            // Check Primary face first. If face is blocked, try secondary.
            if (!resolverHelper.TryResolveByFace(illusion, tile, collisionResolution.PrimaryFace))
            {
                if (!resolverHelper.TryResolveByFace(illusion, tile, collisionResolution.SecondaryFace))
                {
                    return true;
                }
            }

            // resolution was to push it up/down, fall sum goes to 0
            if (illusion.Y != beforeY)
            {
                illusion.FallSum = 0;
            }

            return true;
        }
    }
}
