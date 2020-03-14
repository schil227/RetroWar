using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionFinder
    {
        List<CollisionResolution> FindCollisions(Sprite normal, Sprite based, HitBox normalHitBox, HitBox basedHitBox, bool withRespectToNormal);
    }
}
