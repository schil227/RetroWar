using Newtonsoft.Json;
using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Common;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
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
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public List<EnemyVehicle> EnemyVehicles { get; set; } = new List<EnemyVehicle>();

        [JsonIgnoreAttribute]
        public Dictionary<Tuple<int, int>, GridContainer> Grids { get; set; }
    }
}
