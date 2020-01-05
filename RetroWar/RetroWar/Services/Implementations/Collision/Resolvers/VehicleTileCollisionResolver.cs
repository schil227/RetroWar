﻿using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class VehicleTileCollisionResolver : ICollisionResolver
    {
        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Vehicle vehicle = null;
            Tile tile = null;

            if (normal is Vehicle && based is Tile)
            {
                vehicle = (Vehicle)normal;
                tile = (Tile)based;
            }
            else if (normal is Tile && based is Vehicle)
            {
                vehicle = (Vehicle)based;
                tile = (Tile)normal;
            }
            else
            {
                return false;
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

            return true;
        }
    }
}