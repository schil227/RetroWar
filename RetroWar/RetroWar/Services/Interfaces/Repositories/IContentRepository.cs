using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Actions;
using RetroWar.Models.Repositories.Bullets;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Repositories.Tiles;
using RetroWar.Models.Repositories.Vehicles;
using RetroWar.Models.Screen;
using RetroWar.Models.Sprites.Vehicles;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Repositories
{
    public interface IContentRepository
    {
        IEnumerable<ActionDataDatabaseItem> Actions { get; set; }
        IEnumerable<PlayerVehicleDatabaseItem> PlayerVehicles { get; set; }
        IEnumerable<EnemyVehicleDatabaseItem> EnemyVehicles { get; set; }
        IEnumerable<TextureDatabaseItem> Textures { get; set; }
        IEnumerable<TileDatabaseItem> Tiles { get; set; }
        IEnumerable<BulletDatabaseItem> Bullets { get; set; }
        Stage CurrentStage { get; set; }
        Screen Screen { get; set; }
        Vehicle PlayerTank { get; set; }
    }
}
