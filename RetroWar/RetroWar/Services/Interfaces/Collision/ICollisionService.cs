using RetroWar.Models.Sprites;

namespace RetroWar.Services.Interfaces.Collision
{
    public interface ICollisionService
    {
        bool HandleCollision(Sprite normal, Sprite based, float deltaTime);
    }
}
