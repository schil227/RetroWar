﻿using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Sprites.Vehicles;
using System.Collections.Generic;

namespace RetroWar.Models.Collisions.Grid
{
    public class GridContainer
    {
        public Vehicle playerTank { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; }
        public Dictionary<string, Bullet> Bullets { get; set; }
    }
}
