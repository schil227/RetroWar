using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class IllusionTileCollisionResolver : ICollisionResolver
    {
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

            if (collisionResolution.CollisionPoint == PointInCollision.TwoPoints)
            {
                if (collisionResolution.DeltaX != 0)
                {
                    illusion.X += collisionResolution.DeltaX;
                }
                else
                {
                    illusion.Y += collisionResolution.DeltaY;
                }
            }
            else if (Math.Abs(collisionResolution.DeltaX) < Math.Abs(collisionResolution.DeltaY))
            {
                if (collisionResolution.DeltaX > 0)
                {
                    if (!tile.HasTileToRight)
                    {
                        illusion.X += collisionResolution.DeltaX;
                    }
                    else
                    {
                        illusion.Y += collisionResolution.DeltaY;
                    }
                }
                else
                {
                    if (!tile.HasTileToLeft)
                    {
                        illusion.X += collisionResolution.DeltaX;
                    }
                    else
                    {
                        illusion.Y += collisionResolution.DeltaY;
                    }
                }
            }
            else
            {
                if (collisionResolution.DeltaY > 0)
                {
                    if (!tile.HasTileBelow)
                    {
                        illusion.Y += collisionResolution.DeltaY;
                    }
                    else
                    {
                        illusion.X += collisionResolution.DeltaX;
                    }
                }
                else
                {
                    if (!tile.HasTileAbove)
                    {
                        illusion.Y += collisionResolution.DeltaY;
                    }
                    else
                    {
                        illusion.X += collisionResolution.DeltaX;
                    }
                }
            }

            // resolution was to push it up by landing on a tile; no longer falling.
            if (illusion.Y < beforeY)
            {
                illusion.FallSum = 0;
            }

            return true;
        }
    }
}
