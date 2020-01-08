using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision.Grid
{
    public interface IGridHandler
    {
        Dictionary<Tuple<int, int>, GridContainer> InitializeGrid(Vehicle playerTank, IEnumerable<EnemyVehicle> enemyVehicles, IEnumerable<Tile> tiles);
        void MoveSprite(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, GridContainerSpriteType spritetype, int oldX, int oldY);
        void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, GridContainerSpriteType spritetype, int oldX, int oldY);
        IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY);
    }
}
