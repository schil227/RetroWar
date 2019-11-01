using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class CollisionResolver : ICollisionResolver
    {
        public Resolution ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            if (based is Tile)
            {
                var tile = (Tile)based;

                if (Math.Abs(collisionResolution.DeltaX) < Math.Abs(collisionResolution.DeltaY))
                {
                    if (collisionResolution.DeltaX > 0)
                    {
                        if (!tile.HasTileToRight)
                        {
                            normal.X += collisionResolution.DeltaX;
                            return Resolution.Moved;
                        }
                        else
                        {
                            normal.Y += collisionResolution.DeltaY;
                            return Resolution.Moved;
                        }
                    }
                    else
                    {
                        if (!tile.HasTileToLeft)
                        {
                            normal.X += collisionResolution.DeltaX;
                            return Resolution.Moved;
                        }
                        else
                        {
                            normal.Y += collisionResolution.DeltaY;
                            return Resolution.Moved;
                        }
                    }
                }
                else
                {
                    if (collisionResolution.DeltaY > 0)
                    {
                        if (!tile.HasTileBelow)
                        {
                            normal.Y += collisionResolution.DeltaY;
                            return Resolution.Moved;
                        }
                        else
                        {
                            normal.X += collisionResolution.DeltaX;
                            return Resolution.Moved;
                        }
                    }
                    else
                    {
                        if (!tile.HasTileAbove)
                        {
                            normal.Y += collisionResolution.DeltaY;
                            return Resolution.Moved;
                        }
                        else
                        {
                            normal.X += collisionResolution.DeltaX;
                            return Resolution.Moved;
                        }
                    }
                }
            }

            if (Math.Abs(collisionResolution.DeltaX) < Math.Abs(collisionResolution.DeltaY))
            {
                normal.X += collisionResolution.DeltaX;
            }
            else
            {
                normal.Y += collisionResolution.DeltaY;
            }

            return Resolution.Moved;
        }
    }
}
