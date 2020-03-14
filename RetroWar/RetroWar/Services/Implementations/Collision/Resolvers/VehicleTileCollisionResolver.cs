using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class VehicleTileCollisionResolver : ICollisionResolver
    {
        private readonly ICollisionResolutionHelper collisionResolutionHelper;

        public VehicleTileCollisionResolver
            (
                ICollisionResolutionHelper collisionResolutionHelper
            )
        {
            this.collisionResolutionHelper = collisionResolutionHelper;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Vehicle vehicle = null;
            Tile tile = null;

            if (normal is Vehicle && based is Tile)
            {
                vehicle = (Vehicle)normal;
                tile = (Tile)based;

                if (!collisionResolution.WithRespectToNormal)
                {
                    collisionResolutionHelper.InvertCollisionResolution(collisionResolution);
                }
            }
            else if (normal is Tile && based is Vehicle)
            {
                vehicle = (Vehicle)based;
                tile = (Tile)normal;

                if (collisionResolution.WithRespectToNormal)
                {
                    collisionResolutionHelper.InvertCollisionResolution(collisionResolution);
                }
            }
            else
            {
                return false;
            }

            var beforeY = vehicle.Y;

            if (collisionResolution.CollisionPoint == PointInCollision.TwoPoints)
            {
                if (collisionResolution.DeltaX != 0)
                {
                    vehicle.X += collisionResolution.DeltaX;
                }
                else
                {
                    vehicle.Y += collisionResolution.DeltaY;
                }
            }
            if (Math.Abs(collisionResolution.DeltaX) < Math.Abs(collisionResolution.DeltaY))
            {
                if (collisionResolution.DeltaX > 0)
                {
                    if (!tile.HasTileToRight)
                    {
                        vehicle.X += collisionResolution.DeltaX;
                    }
                    else
                    {
                        vehicle.Y += collisionResolution.DeltaY;
                    }
                }
                else
                {
                    if (!tile.HasTileToLeft)
                    {
                        vehicle.X += collisionResolution.DeltaX;
                    }
                    else
                    {
                        vehicle.Y += collisionResolution.DeltaY;
                    }
                }
            }
            else
            {
                if (collisionResolution.DeltaY > 0)
                {
                    if (!tile.HasTileBelow)
                    {
                        vehicle.Y += collisionResolution.DeltaY;
                    }
                    else
                    {
                        vehicle.X += collisionResolution.DeltaX;
                    }
                }
                else
                {
                    if (!tile.HasTileAbove)
                    {
                        vehicle.Y += collisionResolution.DeltaY;
                    }
                    else
                    {
                        vehicle.X += collisionResolution.DeltaX;
                    }
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
