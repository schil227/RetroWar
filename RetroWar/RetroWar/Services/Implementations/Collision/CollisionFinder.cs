using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionFinder : ICollisionFinder
    {
        private readonly ISpriteHelper spriteHelper;

        private readonly IFaceHelper faceHelper;

        public CollisionFinder(
            ISpriteHelper spriteHelper,
            IFaceHelper faceHelper
            )
        {
            this.spriteHelper = spriteHelper;
            this.faceHelper = faceHelper;
        }

        public CollisionResolution FindCollisionResolutionFace(Sprite normal, Sprite based, float deltaTime)
        {
            Face? xBasedFace = null;
            Face? yBasedFace = null;

            Face? xNormalFace = null;
            Face? yNormalFace = null;

            var xNormalDistance = normal.X - normal.oldX;
            var yNormalDistance = normal.Y - normal.oldY;

            // if normal is heading right, move it to based's left face (etc.)
            if (xNormalDistance > 0)
            {
                xNormalFace = Face.Right;
                xBasedFace = Face.Left;
            }
            else if (xNormalDistance < 0)
            {
                xNormalFace = Face.Left;
                xBasedFace = Face.Right;
            }

            // if normal is heading down, move it to based's top face (etc.)
            if (yNormalDistance > 0)
            {
                yNormalFace = Face.Bottom;
                yBasedFace = Face.Top;
            }
            else if (yNormalDistance < 0)
            {
                yNormalFace = Face.Top;
                yBasedFace = Face.Bottom;
            }

            if (xBasedFace == null && yBasedFace == null)
            {
                return new CollisionResolution();
            }
            else if (xBasedFace == null)
            {
                return new CollisionResolution { PrimaryFace = yBasedFace };
            }
            else if (yBasedFace == null)
            {
                return new CollisionResolution { PrimaryFace = xBasedFace };
            }

            // Determine which face collided first by 

            var xNormalSpeed = Math.Abs(xNormalDistance) / deltaTime;
            var yNormalSpeed = Math.Abs(yNormalDistance) / deltaTime;

            var xFaceDelta = faceHelper.GetFaceDifference(normal, xNormalFace.Value, based);
            var yFaceDelta = faceHelper.GetFaceDifference(normal, yNormalFace.Value, based);

            var xTimeOfImpact = xFaceDelta / xNormalSpeed;
            var yTimeOfImpact = yFaceDelta / yNormalSpeed;

            if (xTimeOfImpact <= yTimeOfImpact)
            {
                return new CollisionResolution
                {
                    PrimaryFace = xBasedFace,
                    SecondaryFace = yBasedFace
                };
            }
            else
            {
                return new CollisionResolution
                {
                    PrimaryFace = yBasedFace,
                    SecondaryFace = xBasedFace
                };
            }
        }
    }
}
