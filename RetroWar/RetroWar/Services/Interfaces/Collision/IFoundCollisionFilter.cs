using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface IFoundCollisionFilter
    {
        CollisionResolution FilterCollisionsFound(Sprite normal, Sprite based, HitBox normalBox, HitBox basedBox);
    }
}
