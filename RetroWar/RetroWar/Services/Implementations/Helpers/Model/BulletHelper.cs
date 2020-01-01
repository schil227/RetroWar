using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class BulletHelper : IBulletHelper
    {
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
    }
}
