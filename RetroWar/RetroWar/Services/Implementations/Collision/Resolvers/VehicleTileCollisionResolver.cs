using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Collision;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class VehicleTileCollisionResolver : ICollisionResolver
    {
        private readonly IResolverHelper resolverHelper;

        public VehicleTileCollisionResolver
            (
                IResolverHelper resolverHelper
            )
        {
            this.resolverHelper = resolverHelper;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Vehicle vehicle = null;
            Tile tile = null;

            if (normal is Vehicle && based is Tile)
            {
                vehicle = (Vehicle)normal;
                tile = (Tile)based;
            }
            else
            {
                return false;
            }

            var beforeY = vehicle.Y;

            // vehicle is "stuck" in tile, nothing to do.
            if (collisionResolution.PrimaryFace == null && collisionResolution.SecondaryFace == null)
            {
                return true;
            }

            // Check Primary face first. If face is blocked, try secondary.
            if (!resolverHelper.TryResolveByFace(vehicle, tile, collisionResolution.PrimaryFace))
            {
                if (!resolverHelper.TryResolveByFace(vehicle, tile, collisionResolution.SecondaryFace))
                {
                    return true;
                }
            }

            // resolution was to push it up by landing on a tile; no longer falling.
            if (vehicle.Y < beforeY)
            {
                vehicle.FallSum = 0;
                vehicle.IsJumping = false;
            }

            return true;
        }
    }
}
