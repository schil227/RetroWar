﻿using RetroWar.Models.Repositories.Actions;
using RetroWar.Models.Repositories.Bullets;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Repositories.Tiles;
using RetroWar.Models.Repositories.Vehicles;
using System.Collections.Generic;

namespace RetroWar.Models.Repositories
{
    public class ContentDatabase
    {
        public IEnumerable<ActionDataDatabaseItem> Actions { get; set; }
        public IEnumerable<VehicleDatabaseItem> Vehicles { get; set; }
        public IEnumerable<TextureDatabaseItem> Textures { get; set; }
        public IEnumerable<TileDatabaseItem> Tiles { get; set; }
        public IEnumerable<BulletDatabaseItem> Bullets { get; set; }
    }
}