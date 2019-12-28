using RetroWar.Models.Repositories.Actions;
using RetroWar.Models.Repositories.Sprites;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Repositories.Tiles;
using System.Collections.Generic;

namespace RetroWar.Models.Repositories
{
    public class ContentDatabase
    {
        public IEnumerable<ActionDataDatabaseItem> Actions { get; set; }
        public IEnumerable<SpriteDatabaseItem> Sprites { get; set; }
        public IEnumerable<TextureDatabaseItem> Textures { get; set; }
        public IEnumerable<TileDatabaseItem> Tiles { get; set; }
    }
}
