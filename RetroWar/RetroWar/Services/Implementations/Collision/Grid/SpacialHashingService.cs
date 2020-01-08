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
        public void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY)
        {
            var gridContainer = GetGridContainer(gridHash, gridX, gridY);

            switch (spriteType)
            {
                case GridContainerSpriteType.Player:
                    gridContainer.playerTank = (PlayerVehicle)sprite;
                    break;
                case GridContainerSpriteType.Tile:
                    gridContainer.Tiles.Add(sprite.SpriteId, (Tile)sprite);
                    break;
                case GridContainerSpriteType.Bullet:
                    gridContainer.Bullets.Add(sprite.SpriteId, (Bullet)sprite);
                    break;
                case GridContainerSpriteType.Enemy:
                    gridContainer.EnemyVehicles.Add(sprite.SpriteId, (EnemyVehicle)sprite);
                    break;
            }
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY)
        {
            var gridContainer = GetGridContainer(gridHash, gridX, gridY);

            switch (spriteType)
            {
                case GridContainerSpriteType.Player:
                    gridContainer.playerTank = null;
                    break;
                case GridContainerSpriteType.Tile:
                    gridContainer.Tiles.Remove(sprite.SpriteId);
                    break;
                case GridContainerSpriteType.Bullet:
                    gridContainer.Bullets.Remove(sprite.SpriteId);
                    break;
                case GridContainerSpriteType.Enemy:
                    gridContainer.EnemyVehicles.Remove(sprite.SpriteId);
                    break;
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
