using RetroWar.Models.Collisions.Resolvers;
using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision.Resolvers.Helpers
{
    public interface ICarrierRiderResolver
    {
        ResolutionVector GetRiderVerticleResolutionVector(Sprite rider, Sprite carrier);
        ResolutionVector GetRiderHorizontalResolutionVector(Sprite rider, Sprite carrier);
        float DistanceFromFaceToClosestTile(Direction direction, float Magnitude, Sprite sprite);
    }
}
