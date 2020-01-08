using RetroWar.Models.Repositories.Vehicles;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IEnemyVehicleLoader
    {
        EnemyVehicleDatabaseItem[] LoadEnemyVehicles(string enemyVehicleFileName);
    }
}
