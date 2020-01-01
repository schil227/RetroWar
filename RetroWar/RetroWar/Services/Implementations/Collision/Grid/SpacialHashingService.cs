using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Grid;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision.Grid
{
    public class SpacialHashingService : ISpacialHashingService
    {
        public void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY)
        {
            GridContainer gridContainer;

            if (!gridHash.TryGetValue(new Tuple<int, int>(gridX, gridY), out gridContainer))
            {
                gridContainer = new GridContainer
                {
                    PlayerSprite = null,
                    Tiles = new Dictionary<string, Tile>(),
                    Bullets = new Dictionary<string, Bullet>()
                };

                gridHash.Add(new Tuple<int, int>(gridX, gridY), gridContainer);
            }

            switch (spriteType)
            {
                case GridContainerSpriteType.Player:
                    gridContainer.PlayerSprite = sprite;
                    break;
                case GridContainerSpriteType.Tile:
                    gridContainer.Tiles.Add(sprite.SpriteId, (Tile)sprite);
                    break;
                case GridContainerSpriteType.Bullet:
                    gridContainer.Bullets.Add(sprite.SpriteId, (Bullet)sprite);
                    break;
            }
        }

        public GridContainer GetGridContainer(Dictionary<Tuple<int, int>, GridContainer> gridHash, int gridX, int gridY)
        {
            GridContainer containerValue;

            if (!gridHash.TryGetValue(new Tuple<int, int>(gridX, gridY), out containerValue))
            {
                return new GridContainer
                {
                    PlayerSprite = null,
                    Tiles = new Dictionary<string, Tile>(),
                    Bullets = new Dictionary<string, Bullet>()
                };
            }

            return containerValue;
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY)
        {
            GridContainer gridContainer;

            if (!gridHash.TryGetValue(new Tuple<int, int>(gridX, gridY), out gridContainer))
            {
                gridContainer = new GridContainer
                {
                    PlayerSprite = null,
                    Tiles = new Dictionary<string, Tile>(),
                    Bullets = new Dictionary<string, Bullet>()
                };

                gridHash.Add(new Tuple<int, int>(gridX, gridY), gridContainer);
            }

            switch (spriteType)
            {
                case GridContainerSpriteType.Player:
                    gridContainer.PlayerSprite = null;
                    break;
                case GridContainerSpriteType.Tile:
                    gridContainer.Tiles.Remove(sprite.SpriteId);
                    break;
                case GridContainerSpriteType.Bullet:
                    gridContainer.Bullets.Remove(sprite.SpriteId);
                    break;
            }
        }
    }
}
