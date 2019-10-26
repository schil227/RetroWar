using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Services.Interfaces.Collision;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionFinder : ICollisionFinder
    {
        public List<CollisionResolution> FindCollisions(Sprite normal, Sprite based, HitBox normalHitBox, HitBox basedHitBox)
        {
            var collisionsFound = new List<CollisionResolution>();

            var nX = normal.X + normalHitBox.RelativeX;
            var nMaxX = normal.X + normalHitBox.RelativeX + normalHitBox.Width;

            var nY = normal.Y + normalHitBox.RelativeY;
            var nMaxY = normal.Y + normalHitBox.RelativeY + normalHitBox.Height;

            var bX = based.X + basedHitBox.RelativeX;
            var bMaxX = based.X + basedHitBox.RelativeX + basedHitBox.Width;

            var bY = based.Y + basedHitBox.RelativeY;
            var bMaxY = based.Y + basedHitBox.RelativeY + basedHitBox.Height;

            /*
             * (nX,nY)   nMaxX
             *  └─> ┌────┐
             *      | N ┌┼───┐
             *nMaxY └───┼* B |
             *          └────┘
             * 
             * Bottom right corner of normal in base
             */
            if (
                nX <= bX && bX < nMaxX &&
                nY <= bY && bY < nMaxY
                )
            {
                collisionsFound.Add(
                    new CollisionResolution
                    {
                        CollisionPoint = PointInCollision.BottomRight,
                        DeltaX = bX - nMaxX,
                        DeltaY = bY - nMaxY
                    });
            }

            /*
             *     ┌────┐
             * ┌───┼┐ N |
             * | B *┼───┘
             * └────┘
             * Bottom left corner of normal in base
             */
            if (nX <= bMaxX && bMaxX < nMaxX &&
               nY <= bY && bY < nMaxY)
            {
                collisionsFound.Add(
                    new CollisionResolution
                    {
                        CollisionPoint = PointInCollision.BottomLeft,
                        DeltaX = bMaxX - nX,
                        DeltaY = bY - nMaxY
                    });
            }

            /*
             * ┌────┐
             * | B *┼───┐
             * └───┼┘ N |
             *     └────┘
             * 
             * Top left corner of normal in base
             */
            if (nX <= bMaxX && bMaxX < nMaxX &&
                nY <= bMaxY && bMaxY < nMaxY)
            {
                collisionsFound.Add(new CollisionResolution
                {
                    CollisionPoint = PointInCollision.TopLeft,
                    DeltaX = bMaxX - nX,
                    DeltaY = bMaxY - nY
                });
            }
            /*
            *     ┌────┐
            * ┌───┼* B |
            * | N └┼───┘
            * └────┘
            * Top left corner of normal in base
            */
            if (nX <= bX && bX < nMaxX &&
                nY <= bMaxY && bMaxY < nMaxY)
            {
                collisionsFound.Add(new CollisionResolution
                {
                    CollisionPoint = PointInCollision.TopLeft,
                    DeltaX = bX - nMaxX,
                    DeltaY = bMaxY - nY
                });
            }

            return collisionsFound;
        }
    }
}
