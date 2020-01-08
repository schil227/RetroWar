using RetroWar.Models.Sprites.Vehicles;

namespace RetroWar.Models.Vehicles.Vehicles.EnemyVehicle
{
    public class EnemyVehicle : Vehicle
    {
        public AIBehavior Behavior { get; set; }
        public bool IsBoss { get; set; }
    }
}
