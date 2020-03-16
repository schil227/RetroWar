using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionChecker : ICollisionChecker
    {
        private readonly IFaceHelper faceHelper;

        public CollisionChecker(IFaceHelper faceHelper)
        {
            this.faceHelper = faceHelper;
        }

        public bool AreColliding(Sprite normal, Sprite based)
        {
            return faceHelper.GetFaceAxis(normal, Face.Left) < faceHelper.GetFaceAxis(based, Face.Right)
                && faceHelper.GetFaceAxis(normal, Face.Right) > faceHelper.GetFaceAxis(based, Face.Left)
                && faceHelper.GetFaceAxis(normal, Face.Top) < faceHelper.GetFaceAxis(based, Face.Bottom)
                && faceHelper.GetFaceAxis(normal, Face.Bottom) > faceHelper.GetFaceAxis(based, Face.Top);
        }
    }
}
