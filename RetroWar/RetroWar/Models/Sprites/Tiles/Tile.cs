namespace RetroWar.Models.Sprites.Tiles
{
    public class Tile : Sprite
    {
        public bool HasTileAbove { get; set; }
        public bool HasTileBelow { get; set; }
        public bool HasTileToLeft { get; set; }
        public bool HasTileToRight { get; set; }
    }
}
