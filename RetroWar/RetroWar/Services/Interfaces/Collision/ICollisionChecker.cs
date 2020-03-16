using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionChecker
    {
        bool AreColliding(Sprite normal, Sprite based);
    }
}
