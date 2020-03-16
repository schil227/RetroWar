using RetroWar.Models.Sprites;

namespace RetroWar.Models.Collisions
{
    public class CollisionResolution
    {
        public PointInCollision CollisionPoint;
        public float DeltaX;
        public float DeltaY;
        public bool WithRespectToNormal;
        public Face? PrimaryFace;
        public Face? SecondaryFace;
    }
}
