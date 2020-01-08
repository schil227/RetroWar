using RetroWar.Models.Repositories.Vehicles;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IPlayerVehicleLoader
    {
        PlayerVehicleDatabaseItem[] LoadPlayerVehicles(string playerVehicleFileName);
    }
}
