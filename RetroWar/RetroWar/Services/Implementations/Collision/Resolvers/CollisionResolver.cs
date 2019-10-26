using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class CollisionResolver : ICollisionResolver
    {
        public Resolution ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution, bool withRespectToX)
        {
            //if (collisionResolution.CollisionPoint == PointInCollision.TwoPoints)
            //{
            //    if (Math.Abs(collisionResolution.DeltaX) > Math.Abs(collisionResolution.DeltaY))
            //    {
            //        normal.X += collisionResolution.DeltaX;
            //    }
            //    else
            //    {
            //        normal.Y += collisionResolution.DeltaY;
            //    }

            //    return Resolution.Moved;
            //}

            if (Math.Abs(collisionResolution.DeltaX) < Math.Abs(collisionResolution.DeltaY))
            {
                normal.X += collisionResolution.DeltaX;
            }
            else
            {
                normal.Y += collisionResolution.DeltaY;
            }

            ////This needs to actually be figured out
            //if (withRespectToX)
            //{
            //    normal.X += collisionResolution.DeltaX;
            //}
            //else
            //{
            //    normal.Y += collisionResolution.DeltaY;
            //}


            return Resolution.Moved;
        }
    }
}
