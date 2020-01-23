using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Actions;
using RetroWar.Models.Repositories.Bullets;
using RetroWar.Models.Repositories.Illusions;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Repositories.Tiles;
using RetroWar.Models.Repositories.Vehicles;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Repositories;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Repositories
{
    public class ContentRepository : IContentRepository
    {
        public IEnumerable<PlayerVehicleDatabaseItem> PlayerVehicles { get; set; }
        public IEnumerable<EnemyVehicleDatabaseItem> EnemyVehicles { get; set; }
        public IEnumerable<ActionDataDatabaseItem> Actions { get; set; }
        public IEnumerable<TextureDatabaseItem> Textures { get; set; }
        public IEnumerable<TileDatabaseItem> Tiles { get; set; }
        public IEnumerable<BulletDatabaseItem> Bullets { get; set; }
        public IEnumerable<IllusionDatabaseItem> Illusions { get; set; }
        public Stage CurrentStage { get; set; }
        public Screen Screen { get; set; }
        public Vehicle PlayerTank { get; set; }
    }
}
