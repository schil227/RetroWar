using RetroWar.Exceptions.Implementations.Collision;
using RetroWar.Models.Collisions;
using RetroWar.Services.Interfaces.Collision;
using System.Collections.Generic;
using System.Linq;

namespace RetroWar.Services.Implementations.Collision
{
    public class MultiPointCollisionResolver : IMultiPointCollisionResolver
    {
        private readonly List<PointInCollision> topCollision;
        private readonly List<PointInCollision> bottomCollision;
        private readonly List<PointInCollision> leftCollision;
        private readonly List<PointInCollision> rightCollision;

        public MultiPointCollisionResolver()
        {
            topCollision = new List<PointInCollision> { PointInCollision.TopLeft, PointInCollision.TopRight };
            bottomCollision = new List<PointInCollision> { PointInCollision.BottomLeft, PointInCollision.BottomRight };
            leftCollision = new List<PointInCollision> { PointInCollision.TopLeft, PointInCollision.BottomLeft };
            rightCollision = new List<PointInCollision> { PointInCollision.TopRight, PointInCollision.BottomRight };
        }

        public CollisionResolution ResolveFourPointCollision(List<CollisionResolution> collisions)
        {
            var smallestDeltaX = collisions.ElementAt(0).DeltaX;
            var smallestDeltaY = collisions.ElementAt(0).DeltaY;

            foreach (var collision in collisions)
            {
                if (collision.DeltaX < smallestDeltaX)
                {
                    smallestDeltaX = collision.DeltaX;
                }

                if (collision.DeltaY < smallestDeltaY)
                {
                    smallestDeltaY = collision.DeltaY;
                }
            }

            return new CollisionResolution
            {
                CollisionPoint = PointInCollision.FourPoints,
                DeltaX = smallestDeltaX,
                DeltaY = smallestDeltaY
            };
        }

        public CollisionResolution ResolveTwoPointCollision(List<CollisionResolution> collisions)
        {
            if (collisions.Count != 2)
            {
                throw new MultiPointCollisionResolverException($"Incorrect number of collisions to consolidate. Number of collisions: {collisions.Count}");
            }

            var first = collisions.ElementAt(0);
            var second = collisions.ElementAt(1);

            var points = new List<PointInCollision> { first.CollisionPoint, second.CollisionPoint };

            if (points.Except(topCollision).ToList().Count == 0 ||
                points.Except(bottomCollision).ToList().Count == 0)
            {
                return new CollisionResolution
                {
                    CollisionPoint = PointInCollision.TwoPoints,
                    DeltaX = 0,
                    DeltaY = first.DeltaY
                };
            }

            if (points.Except(leftCollision).ToList().Count == 0 ||
                points.Except(rightCollision).ToList().Count == 0)
            {
                return new CollisionResolution
                {
                    CollisionPoint = PointInCollision.TwoPoints,
                    DeltaX = first.DeltaX,
                    DeltaY = 0
                };
            }

            throw new MultiPointCollisionResolverException($"Invalid collection of collisions. Collisions: {string.Join(", ", points)}");
        }
    }
}
