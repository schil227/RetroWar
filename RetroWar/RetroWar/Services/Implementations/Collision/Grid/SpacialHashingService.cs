using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Collision.Grid;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision.Grid
{
    public class SpacialHashingService : ISpacialHashingService
    {
        public void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, int gridX, int gridY)
        {
            var gridContainer = GetGridContainer(gridHash, gridX, gridY);

            if (sprite is PlayerVehicle)
            {
                gridContainer.playerTank = (PlayerVehicle)sprite;
            }
            else if (sprite is EnemyVehicle)
            {
                gridContainer.EnemyVehicles.Add(sprite.SpriteId, (EnemyVehicle)sprite);
            }
            else if (sprite is Bullet)
            {
                gridContainer.Bullets.Add(sprite.SpriteId, (Bullet)sprite);
            }
            else if (sprite is Tile)
            {
                gridContainer.Tiles.Add(sprite.SpriteId, (Tile)sprite);
            }
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, int gridX, int gridY)
        {
            var gridContainer = GetGridContainer(gridHash, gridX, gridY);

            if (sprite is PlayerVehicle)
            {
                gridContainer.playerTank = null;
            }
            else if (sprite is EnemyVehicle)
            {
                gridContainer.EnemyVehicles.Remove(sprite.SpriteId);
            }
            else if (sprite is Bullet)
            {
                gridContainer.Bullets.Remove(sprite.SpriteId);
            }
            else if (sprite is Tile)
            {
                gridContainer.Tiles.Remove(sprite.SpriteId);
            }
        }

        public GridContainer GetGridContainer(Dictionary<Tuple<int, int>, GridContainer> gridHash, int gridX, int gridY)
        {
            GridContainer containerValue;

            if (!gridHash.TryGetValue(new Tuple<int, int>(gridX, gridY), out containerValue))
            {
                containerValue = new GridContainer
                {
                    playerTank = null,
                    Tiles = new Dictionary<string, Tile>(),
                    Bullets = new Dictionary<string, Bullet>(),
                    EnemyVehicles = new Dictionary<string, EnemyVehicle>()
                };

                gridHash.Add(new Tuple<int, int>(gridX, gridY), containerValue);
            }

            return containerValue;
        }

    }
}
