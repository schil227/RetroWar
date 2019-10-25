using RetroWar.Models.Collisions;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface IMultiPointCollisionResolver
    {
        CollisionResolution ResolveTwoPointCollision(List<CollisionResolution> collisions);
        CollisionResolution ResolveFourPointCollision(List<CollisionResolution> collisions);
    }
}
