﻿using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision.Grid
{
    public interface IGridService
    {
        void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite);
        void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite);
        IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY);
    }
}
