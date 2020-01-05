using RetroWar.Models.Repositories.Vehicles;

namespace RetroWar.Services.Interfaces.Loaders
{
    public interface IVehicleLoader
    {
        VehicleDatabaseItem[] LoadVehicles(string vehicleFileName);
    }
}
