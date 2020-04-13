using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Level;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
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

        public void InitializeGrid(Stage stage, Vehicle playerTank, IEnumerable<EnemyVehicle> enemyVehicles)
        {
            stage.Grids = new Dictionary<Tuple<int, int>, GridContainer>();

            gridService.AddSpriteToGrid(stage.Grids, playerTank);

            foreach (var tile in stage.Tiles)
            {
                gridService.AddSpriteToGrid(stage.Grids, tile);
            }

            foreach (var enemy in enemyVehicles)
            {
                gridService.AddSpriteToGrid(stage.Grids, enemy);
            }
        }

        public void MoveSprite(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite)
        {
            gridService.RemoveSpriteFromGrid(gridHash, sprite);
            gridService.AddSpriteToGrid(gridHash, sprite);
        }

        public IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY)
        {
            return gridService.GetGridsFromPoints(gridHash, x, y, maxX, maxY);
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite)
        {
            gridService.RemoveSpriteFromGrid(gridHash, sprite);
        }
    }
}
