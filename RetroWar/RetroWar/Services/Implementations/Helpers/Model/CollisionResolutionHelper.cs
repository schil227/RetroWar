using RetroWar.Models.Collisions;
using RetroWar.Services.Interfaces.Helpers.Model;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class CollisionResolutionHelper : ICollisionResolutionHelper
    {
        public void InvertCollisionResolution(CollisionResolution collisionResolution)
        {
            collisionResolution.WithRespectToNormal = !collisionResolution.WithRespectToNormal;
            collisionResolution.DeltaX *= -1;
            collisionResolution.DeltaY *= -1;

            switch (collisionResolution.CollisionPoint)
            {
                case PointInCollision.BottomLeft:
                    collisionResolution.CollisionPoint = PointInCollision.TopRight;
                    break;
                case PointInCollision.BottomRight:
                    collisionResolution.CollisionPoint = PointInCollision.TopLeft;
                    break;
                case PointInCollision.TopRight:
                    collisionResolution.CollisionPoint = PointInCollision.BottomLeft;
                    break;
                case PointInCollision.TopLeft:
                    collisionResolution.CollisionPoint = PointInCollision.BottomRight;
                    break;
            }
        }
    }
}
