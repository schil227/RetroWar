using Newtonsoft.Json;

namespace RetroWar.Models.Sprites.HitBoxes
{
    public class HitBox
    {
        public HitBox(int RelativeX, int RelativeY, int Width, int Height)
        {
            this.RelativeX = RelativeX;
            this.RelativeY = RelativeY;
            this.Width = Width;
            this.Height = Height;
        }

        public bool IsEmptyHitbox
        {
            get
            {
                return this.Width == 0 || this.Height == 0;
            }
        }

        [JsonProperty]
        public int RelativeX { get; private set; }

        [JsonProperty]
        public int RelativeY { get; private set; }

        [JsonProperty]
        public int Width { get; private set; }

        [JsonProperty]
        public int Height { get; private set; }
    }
}
