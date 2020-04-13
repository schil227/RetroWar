using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Level;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision.Grid
{
    public interface IGridHandler
    {
        void InitializeGrid(Stage stage, Vehicle playerTank, IEnumerable<EnemyVehicle> enemyVehicles);
        void MoveSprite(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite);
        void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite);
        IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY);
    }
}
