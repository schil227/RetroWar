using RetroWar.Exceptions.Implementations.Collision.Resolvers.Helpers;
using RetroWar.Models.Collisions.Resolvers;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Resolvers.Helpers;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Repositories;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers.Helpers
{
    public class CarrierRiderResolver : ICarrierRiderResolver
    {
        private readonly IFaceHelper faceHelper;
        private readonly IContentRepository contentRepository;
        private readonly ISpriteHelper spriteHelper;

        public CarrierRiderResolver
            (
            IFaceHelper faceHelper,
            IContentRepository contentRepository,
            ISpriteHelper spriteHelper
            )
        {
            this.faceHelper = faceHelper;
            this.contentRepository = contentRepository;
            this.spriteHelper = spriteHelper;
        }

        public ResolutionVector GetRiderVerticleResolutionVector(Sprite rider, Sprite carrier)
        {
            var difference = faceHelper.GetFaceDifference(rider, Face.Bottom, carrier);

            return new ResolutionVector
            {
                Direction = Direction.Up,
                Magnitude = difference
            };
        }

        public ResolutionVector GetRiderHorizontalResolutionVector(Sprite rider, Sprite carrier)
        {
            var deltaX = carrier.X - carrier.OldX;

            return new ResolutionVector
            {
                Direction = deltaX > 0 ? Direction.Right : Direction.Left,
                Magnitude = Math.Abs(deltaX)
            };
        }

        public float DistanceFromFaceToClosestTile(Direction direction, float magnitude, Sprite sprite)
        {
            var hitBox = spriteHelper.GetHitBox(sprite);

            switch (direction)
            {
                case Direction.Up:
                    {

                        var xStart = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width);
                        var xEnd = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width) + hitBox.Width;

                        var axisStart = sprite.Y + (hitBox.RelativeY);

                        var closestTileFaceAxis = FindClosestTileAxis(Face.Bottom, xStart, xEnd, axisStart, axisStart - magnitude);

                        return Math.Abs(Math.Abs(axisStart) - Math.Abs(closestTileFaceAxis));
                    }
                case Direction.Down:
                    {
                        var xStart = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width);
                        var xEnd = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width) + hitBox.Width;

                        var axisStart = sprite.Y + (hitBox.RelativeY) + hitBox.Height;

                        var closestTileFaceAxis = FindClosestTileAxis(Face.Top, xStart, xEnd, axisStart, axisStart + magnitude);

                        return Math.Abs(Math.Abs(axisStart) - Math.Abs(closestTileFaceAxis));
                    }
                case Direction.Left:
                    {
                        var yStart = sprite.Y + hitBox.RelativeY;
                        var yEnd = sprite.Y + hitBox.RelativeY + hitBox.Height;

                        var axisStart = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width);

                        var closestTileFaceAxis = FindClosestTileAxis(Face.Right, yStart, yEnd, axisStart, axisStart - magnitude);

                        return Math.Abs(Math.Abs(axisStart) - Math.Abs(closestTileFaceAxis));
                    }
                case Direction.Right:
                    {
                        var yStart = sprite.Y + hitBox.RelativeY;
                        var yEnd = sprite.Y + hitBox.RelativeY + hitBox.Height;

                        var axisStart = sprite.X + spriteHelper.GetHitboxXOffset(sprite, hitBox.RelativeX, hitBox.Width) + hitBox.Width;

                        var closestTileFaceAxis = FindClosestTileAxis(Face.Left, yStart, yEnd, axisStart, axisStart + magnitude);

                        return Math.Abs(Math.Abs(axisStart) - Math.Abs(closestTileFaceAxis));
                    }
            }

            throw new CarrierRiderResolverException("Illegial direction specified.");

        }

        private float FindClosestTileAxis(Face tileFace, float broadStart, float broadEnd, float axisStart, float deltaAxis)
        {
            var stage = contentRepository.CurrentStage;

            var deltaStep = deltaAxis < axisStart ? -1 : 1;

            var deltaStart = GetTileCoord(axisStart + deltaStep * 16);

            // if the deltaAxis is closer than axisStart +/- 16, just start at the deltaAxis and do one loop
            if (Math.Abs(axisStart - deltaAxis) < 16)
            {
                deltaStart = GetTileCoord(deltaAxis);
            }

            var deltaEnd = GetTileCoord(deltaAxis);

            // The broad side pixels are moved in a little to prevent clipping an adjsent sprite
            // e.g. player pushing enemy tank on the ground BEFORE enemy tank is pushed up by gravity
            //      (So the enemy tank is clipping in the ground). +/- 4 makes it so the clipped tile
            //      Isn't picked up.
            var adjBroadStart = Math.Min(GetTileCoord(broadStart + 4), GetTileCoord(broadEnd));
            var adjBroadEnd = Math.Min(GetTileCoord(broadEnd - 4), GetTileCoord(broadStart));

            for (var i = deltaStart; IsPastFinalStep(i, deltaEnd, deltaStep); i += deltaStep)
            {
                for (var j = adjBroadStart; j <= adjBroadEnd; j++)
                {
                    var tuple = new Tuple<int, int>(i, j);

                    // when delta is y-based, flip i/j values (j = x based, i = y based)
                    if (tileFace == Face.Bottom || tileFace == Face.Top)
                    {
                        tuple = new Tuple<int, int>(j, i);
                    }

                    if (stage.TileLookup.TryGetValue(tuple, out var tile))
                    {
                        return faceHelper.GetFaceAxis(tile, tileFace);
                    }
                }
            }

            return deltaAxis;
        }

        private bool IsPastFinalStep(int i, int deltaEnd, int deltaStep)
        {
            if (deltaStep > 0)
            {
                return i <= deltaEnd;
            }
            else
            {
                return i >= deltaEnd;
            }
        }

        private int GetTileCoord(float position)
        {
            // negative values need to be pushed to the previous tile, 
            // since a position of, say -5 would otherwise put them in the wrong tile
            // -5/16  => tile 0
            // subtracting 1 when the postion is negative corrects this:
            // (-5/16) - 1 => tile -1
            // box:... -2          -1          0        1         2  ...
            // posn:   [-32, -16]  [-16, -0]  [0, 16]   [16, 32]  [32, 48]
            return (int)(position > 0 ? position / 16 : (position / 16) - 1);
        }
    }
}
