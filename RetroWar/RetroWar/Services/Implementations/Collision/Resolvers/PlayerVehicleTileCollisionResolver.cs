using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Collision;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class PlayerVehicleTileCollisionResolver : ICollisionResolver
    {
        private readonly IResolverHelper resolverHelper;

        public PlayerVehicleTileCollisionResolver
            (
                IResolverHelper resolverHelper
            )
        {
            this.resolverHelper = resolverHelper;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            PlayerVehicle playerVehicle = null;
            Tile tile = null;

            if (normal is PlayerVehicle && based is Tile)
            {
                playerVehicle = (PlayerVehicle)normal;
                tile = (Tile)based;
            }
            else
            {
                return false;
            }

            var beforeY = playerVehicle.Y;

            // vehicle is "stuck" in tile, nothing to do.
            if (collisionResolution.PrimaryFace == null && collisionResolution.SecondaryFace == null)
            {
                return true;
            }

            // Check Primary face first. If face is blocked, try secondary.
            if (!resolverHelper.TryResolveByFace(playerVehicle, tile, collisionResolution.PrimaryFace))
            {
                if (!resolverHelper.TryResolveByFace(playerVehicle, tile, collisionResolution.SecondaryFace))
                {
                    // Resolution impossible with this vehicle/tile combination; continue to next.
                    return true;
                }
            }

            // resolution was to push it up by landing on a tile; no longer falling.
            if (playerVehicle.Y < beforeY)
            {
                playerVehicle.FallSum = 0;
                playerVehicle.IsJumping = false;
            }

            return true;
        }
    }
}
