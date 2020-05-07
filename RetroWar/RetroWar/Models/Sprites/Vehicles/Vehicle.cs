using System.Collections.Generic;

namespace RetroWar.Models.Sprites.Vehicles
{
    public class Vehicle : Sprite
    {
        public int TotalHealth { get; set; }
        public int Health { get; set; }
        public bool IsJumping { get; set; }
        public float FallSum { get; set; }
        public float FallRate { get; set; }
        public float VehicleSpeed { get; set; }
        public FiringMode CurrentFiringMode { get; set; }
        public Dictionary<FiringMode, FiringData> FiringData { get; set; }
        public Dictionary<string, Face> StickyCollisionData { get; set; } = new Dictionary<string, Face>();
    }
}
