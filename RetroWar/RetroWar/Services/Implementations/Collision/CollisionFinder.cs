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

            var xNormalDistance = normal.X - normal.OldX;
            var yNormalDistance = normal.Y - normal.OldY;

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
                return new CollisionResolution { DeltaTime = deltaTime };
            }
            else if (xBasedFace == null)
            {
                return new CollisionResolution { PrimaryFace = yBasedFace, DeltaTime = deltaTime };
            }
            else if (yBasedFace == null)
            {
                return new CollisionResolution { PrimaryFace = xBasedFace, DeltaTime = deltaTime };
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
                    SecondaryFace = yBasedFace,
                    DeltaTime = deltaTime
                };
            }
            else
            {
                return new CollisionResolution
                {
                    PrimaryFace = yBasedFace,
                    SecondaryFace = xBasedFace,
                    DeltaTime = deltaTime
                };
            }
        }

        public bool IsOnTopOf(Sprite normal, Sprite based)
        {
            var normalPreviousBottom = faceHelper.GetFaceAxis(normal, normal.OldY, Face.Bottom);
            var normalCurrentBottom = faceHelper.GetFaceAxis(normal, Face.Bottom);
            var basedCurrentTop = faceHelper.GetFaceAxis(based, Face.Top);

            if (normalPreviousBottom <= basedCurrentTop && basedCurrentTop < normalCurrentBottom)
            {
                return true;
            }

            return false;
        }

        public bool IsRightOf(Sprite normal, Sprite based)
        {
            var previousAxis = faceHelper.GetFaceAxis(normal, normal.OldX, Face.Left);
            var currentAxis = faceHelper.GetFaceAxis(normal, Face.Left);
            var comparableAxis = faceHelper.GetFaceAxis(based, Face.Right);

            // If normal's not moving, do it wrt based
            if (normal.X == normal.OldX)
            {
                previousAxis = faceHelper.GetFaceAxis(based, based.OldX, Face.Right);
                currentAxis = faceHelper.GetFaceAxis(based, Face.Right);
                comparableAxis = faceHelper.GetFaceAxis(normal, Face.Left);
            }

            if (previousAxis <= comparableAxis && comparableAxis < currentAxis)
            {
                return true;
            }

            return false;
        }
    }
}
