using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionFinder : ICollisionFinder
    {
        private readonly ISpriteHelper spriteHelper;

        public CollisionFinder(ISpriteHelper spriteHelper)
        {
            this.spriteHelper = spriteHelper;
        }

        public List<CollisionResolution> FindCollisions(Sprite normal, Sprite based, HitBox normalHitBox, HitBox basedHitBox)
        {
            var collisionsFound = new List<CollisionResolution>();

            var normalXOffset = spriteHelper.GetHitboxXOffset(normal, normalHitBox.RelativeX, normalHitBox.Width);
            var baseXOffset = spriteHelper.GetHitboxXOffset(based, basedHitBox.RelativeX, basedHitBox.Width);

            var nX = normal.X + normalXOffset;
            var nMaxX = normal.X + normalXOffset + normalHitBox.Width;

            var nY = normal.Y + normalHitBox.RelativeY;
            var nMaxY = normal.Y + normalHitBox.RelativeY + normalHitBox.Height;

            var bX = based.X + baseXOffset;
            var bMaxX = based.X + baseXOffset + basedHitBox.Width;

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
            if (bX <= nMaxX && nMaxX < bMaxX &&
                bY <= nMaxY && nMaxY < bMaxY)
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
            if (bX <= nX && nX < bMaxX &&
               bY <= nMaxY && nMaxY < bMaxY)
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
            if (bX <= nX && nX < bMaxX &&
              bY <= nY && nY < bMaxY)
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
            * Top right corner of normal in base
            */
            if (bX <= nMaxX && nMaxX < bMaxX &&
               bY <= nY && nY < bMaxY)
            {
                collisionsFound.Add(new CollisionResolution
                {
                    CollisionPoint = PointInCollision.TopRight,
                    DeltaX = bX - nMaxX,
                    DeltaY = bMaxY - nY
                });
            }

            return collisionsFound;
        }
    }
}
