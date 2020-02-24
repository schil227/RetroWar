using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Factories;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class BulletHelper : IBulletHelper
    {
        private readonly ISpriteFactory spriteFactory;

        public BulletHelper
            (
            ISpriteFactory spriteFactory
            )
        {
            this.spriteFactory = spriteFactory;
        }

        public Point FindNextPointInTrajectory(Bullet bullet, float deltaTime)
        {
            var nextPoint = new Point
            {
                X = bullet.X,
                Y = bullet.Y
            };

            if (bullet.Trajectory == Trajectory.Straight)
            {
                var directionVector = bullet.CurrentDirection == Direction.Right ? 1.0f : -1.0f;

                var distance = Math.Min(bullet.Speed * deltaTime, 10);

                nextPoint.X += (distance * directionVector);

                return nextPoint;
            }

            Console.WriteLine("Trajectory not implemented yet. Gonna stay right here.");
            return nextPoint;
        }

        public void FireBullet(Vehicle vehicle, FiringMode firingMode)
        {
            var firingData = vehicle.FiringData[firingMode];

            var directionOffset = vehicle.CurrentDirection == Direction.Right ? firingData.XSpawningOffset : firingData.XSpawningOffset * -1;

            var spawnPoint = new Point
            {
                X = vehicle.X + directionOffset,
                Y = vehicle.Y + firingData.YSpawningOffset
            };

            var damageType = (vehicle is PlayerVehicle ? DamageDiscrimination.DamagesEnemy : DamageDiscrimination.DamagesPlayer);

            spriteFactory.CreateBullet(firingData.BulletId, spawnPoint, vehicle.CurrentDirection, damageType);
        }
    }
}
