namespace RetroWar.Models.Sprites.Illusions
{
    public class Illusion : Sprite
    {
        public bool SubjectToGravity { get; set; }
        public float FallSum { get; set; }
        public float FallRate { get; set; }
    }
}
