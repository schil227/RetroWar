using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Collision.Resolvers;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class BulletPlayerVehicleCollisionResolver : ICollisionResolver
    {
        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            PlayerVehicle playerTank = null;
            Bullet bullet = null;

            if (normal is PlayerVehicle && based is Bullet)
            {
                playerTank = (PlayerVehicle)normal;
                bullet = (Bullet)based;
            }
            else if (normal is Bullet && based is PlayerVehicle)
            {
                playerTank = (PlayerVehicle)based;
                bullet = (Bullet)normal;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
