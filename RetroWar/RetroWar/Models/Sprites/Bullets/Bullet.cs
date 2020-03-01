namespace RetroWar.Models.Sprites.Bullets
{
    public class Bullet : Sprite
    {
        public Trajectory Trajectory;
        public float Speed;
        public float SpecificGravity;
        public double TotalTime;
        public DamageDiscrimination DamageDiscrimination;
        public int Damage;
    }
}
