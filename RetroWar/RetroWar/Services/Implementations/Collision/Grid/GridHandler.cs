using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Grid;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision.Grid
{
    public class GridHandler : IGridHandler
    {
        private readonly IGridService gridService;

        public GridHandler(IGridService gridService)
        {
            this.gridService = gridService;
        }

        public Dictionary<Tuple<int, int>, GridContainer> InitializeGrid(Sprite playerSprite, IEnumerable<Tile> bullets)
        {
            var gridHash = new Dictionary<Tuple<int, int>, GridContainer>();

            gridService.AddSpriteToGrid(gridHash, GridContainerSpriteType.Player, playerSprite);

            foreach (var bullet in bullets)
            {
                gridService.AddSpriteToGrid(gridHash, GridContainerSpriteType.Tile, bullet);
            }

            return gridHash;
        }

        public void MoveSprite(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, GridContainerSpriteType spritetype, int oldX, int oldY)
        {
            gridService.RemoveSpriteFromGrid(gridHash, spritetype, sprite, oldX, oldY);
            gridService.AddSpriteToGrid(gridHash, spritetype, sprite);
        }

        public IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY)
        {
            return gridService.GetGridsFromPoints(gridHash, x, y, maxX, maxY);
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, GridContainerSpriteType spritetype)
        {
            gridService.RemoveSpriteFromGrid(gridHash, spritetype, sprite, (int)sprite.X, (int)sprite.Y);
        }
    }
}
