using RetroWar.Models.Level;
using RetroWar.Models.Repositories.Actions;
using RetroWar.Models.Repositories.Bullets;
using RetroWar.Models.Repositories.Sprites;
using RetroWar.Models.Repositories.Textures;
using RetroWar.Models.Repositories.Tiles;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Repositories;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Repositories
{
    public class ContentRepository : IContentRepository
    {
        public IEnumerable<ActionDataDatabaseItem> Actions { get; set; }
        public IEnumerable<SpriteDatabaseItem> Sprites { get; set; }
        public IEnumerable<TextureDatabaseItem> Textures { get; set; }
        public IEnumerable<TileDatabaseItem> Tiles { get; set; }
        public IEnumerable<BulletDatabaseItem> Bullets { get; set; }
        public Stage CurrentStage { get; set; }
        public Sprite PlayerSprite { get; set; }
    }
}
