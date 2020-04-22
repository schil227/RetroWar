using RetroWar.Models.Collisions.Resolvers;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Resolvers.Helpers;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers.Helpers
{
    public class CarrierRiderResolver : ICarrierRiderResolver
    {
        private readonly IFaceHelper faceHelper;

        public CarrierRiderResolver(IFaceHelper faceHelper)
        {
            this.faceHelper = faceHelper;
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
    }
}
