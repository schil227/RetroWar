using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using System.Collections.Generic;

namespace RetroWar.Models.Collisions.Grid
{
    public class GridContainer
    {
        public PlayerVehicle playerTank { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; }
        public Dictionary<string, Bullet> Bullets { get; set; }
        public Dictionary<string, EnemyVehicle> EnemyVehicles { get; set; }
        public Dictionary<string, Illusion> Illusions { get; set; }
    }
}
