﻿using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Common;
using RetroWar.Models.Sprites.Tiles;
using System;
using System.Collections.Generic;

namespace RetroWar.Models.Level
{
    public class Stage
    {
        public string StageId { get; set; }
        public string Name { get; set; }
        public bool IsUnlocked { get; set; }
        public bool IsCompleted { get; set; }
        public Point PlayerSpawnLocation { get; set; }
        public Point ScreenSpawnLocation { get; set; }
        public Dictionary<Tuple<int, int>, GridContainer> Grids { get; set; }
        public IEnumerable<Tile> Tiles;
    }
}