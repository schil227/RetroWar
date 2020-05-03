using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
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

        public bool IsOnTopOf(Vehicle normal, Vehicle based)
        {
            var velocityDifference = Math.Abs(normal.FallSum) - Math.Abs(based.FallSum);

            if (velocityDifference > 0)
            {
                var previousAxis = faceHelper.GetFaceAxis(normal, normal.OldY, Face.Bottom);
                var currentAxis = faceHelper.GetFaceAxis(normal, Face.Bottom);
                var comparisonAxis = faceHelper.GetFaceAxis(based, Face.Top);

                if (previousAxis <= comparisonAxis + 0.75f && comparisonAxis - 0.75f <= currentAxis)
                {
                    return true;
                }

                var adjustedAxis = comparisonAxis;

                // if based going up and normal going down, use based's old axis
                if (based.FallSum < 0)
                {
                    adjustedAxis = faceHelper.GetFaceAxis(based, based.OldY, Face.Top);
                }

                if (previousAxis <= adjustedAxis + 0.75f && adjustedAxis - 0.75f <= currentAxis)
                {
                    return true;
                }
            }
            else
            {
                var previousAxis = faceHelper.GetFaceAxis(based, based.OldY, Face.Top);
                var currentAxis = faceHelper.GetFaceAxis(based, Face.Top);
                var comparisonAxis = faceHelper.GetFaceAxis(normal, Face.Bottom);

                if (previousAxis >= comparisonAxis - 0.75f && comparisonAxis + 0.75f >= currentAxis)
                {
                    return true;
                }

                var adjustedAxis = comparisonAxis;

                // if normal going down and based going up, use normal's old axis
                if (normal.FallSum > 0)
                {
                    adjustedAxis = faceHelper.GetFaceAxis(normal, normal.OldY, Face.Bottom);
                }

                if (previousAxis >= adjustedAxis - 0.75f && adjustedAxis + 0.75f >= currentAxis)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsRightOf(Sprite normal, Sprite based)
        {
            // If normal's moving, do it wrt to normal. otherwise, wrt based.
            if (normal.X != normal.OldX)
            {
                var previousAxis = faceHelper.GetFaceAxis(normal, normal.OldX, Face.Left);
                var currentAxis = faceHelper.GetFaceAxis(normal, Face.Left);
                var comparableAxis = faceHelper.GetFaceAxis(based, Face.Right);

                if (previousAxis >= comparableAxis - 0.75f && comparableAxis + 0.75f >= currentAxis)
                {
                    return true;
                }
            }
            else
            {
                var previousAxis = faceHelper.GetFaceAxis(based, based.OldX, Face.Right);
                var currentAxis = faceHelper.GetFaceAxis(based, Face.Right);
                var comparableAxis = faceHelper.GetFaceAxis(normal, Face.Left);

                if (previousAxis <= comparableAxis + 0.75f && comparableAxis - 0.75f <= currentAxis)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
