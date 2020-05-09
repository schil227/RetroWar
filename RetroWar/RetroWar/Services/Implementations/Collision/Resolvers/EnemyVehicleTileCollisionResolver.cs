using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Collision;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class EnemyVehicleTileCollisionResolver : ICollisionResolver
    {
        private readonly IResolverHelper resolverHelper;

        public EnemyVehicleTileCollisionResolver
            (
                IResolverHelper resolverHelper
            )
        {
            this.resolverHelper = resolverHelper;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            EnemyVehicle vehicle = null;
            Tile tile = null;

            if (normal is EnemyVehicle && based is Tile)
            {
                vehicle = (EnemyVehicle)normal;
                tile = (Tile)based;
            }
            else
            {
                return false;
            }

            var beforeX = vehicle.X;
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
                    // Resolution impossible with this vehicle/tile combination; continue to next.
                    return true;
                }
            }

            if (vehicle.Y != beforeY)
            {
                //either pushed up/down, fall sum should go to 0.
                vehicle.FallSum = 0;

                // resolution was to push it up by landing on a tile; no longer falling.
                if (vehicle.Y < beforeY)
                {
                    vehicle.IsJumping = false;
                }
            }

            // TODO: introduce composite pattern here for new enemy types
            if (vehicle.Behavior == AIBehavior.MoveLeftAndRight)
            {
                // "Bounce" to other direction when horizontal impact occurs
                if (vehicle.X < beforeX && vehicle.CurrentDirection == Direction.Right)
                {
                    vehicle.CurrentDirection = Direction.Left;
                }
                else if (vehicle.X > beforeX && vehicle.CurrentDirection == Direction.Left)
                {
                    vehicle.CurrentDirection = Direction.Right;
                }
            }

            return true;
        }
    }
}
