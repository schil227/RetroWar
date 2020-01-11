using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;

namespace RetroWar.Services.Interfaces.AI
{
    public interface IBehaviorProcessor
    {
        bool ProcessBehavior(EnemyVehicle enemy, float deltaTime);
    }
}
