using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Helpers.Model
{
    public class GridContainerHelper : IGridContainerHelper
    {
        public HashSet<Sprite> GetActionableGridSprites(GridContainer box)
        {
            var sprites = new HashSet<Sprite>();

            if (
                    box.playerTank == null &&
                    box.Bullets.Count == 0 &&
                    box.EnemyVehicles.Count == 0 &&
                    box.Illusions.Count == 0
                    )
            {
                return sprites;
            }

            if (box.playerTank != null)
            {
                sprites.Add(box.playerTank);
            }

            foreach (var enemy in box.EnemyVehicles)
            {
                sprites.Add(enemy.Value);
            }

            foreach (var bullet in box.Bullets)
            {
                sprites.Add(bullet.Value);
            }

            foreach (var illusion in box.Illusions)
            {
                sprites.Add(illusion.Value);
            }

            foreach (var tile in box.Tiles)
            {
                sprites.Add(tile.Value);
            }

            return sprites;
        }
    }
}
