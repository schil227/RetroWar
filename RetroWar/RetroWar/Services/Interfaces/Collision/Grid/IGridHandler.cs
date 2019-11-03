﻿using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Tiles;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision.Grid
{
    public interface IGridHandler
    {
        Dictionary<Tuple<int, int>, GridContainer> InitializeGrid(Sprite playerSprite, IEnumerable<Tile> tiles);
        void MoveSprite(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, GridContainerSpriteType spritetype, int oldX, int oldY);
        IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY);
    }
}
