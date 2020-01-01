using Newtonsoft.Json;
using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Factories;
using RetroWar.Services.Interfaces.Repositories;
using System;
using System.Linq;

namespace RetroWar.Services.Implementations.Factories
{
    public class SpriteFactory : ISpriteFactory
    {
        private readonly IContentRepository contentRepository;
        private readonly IGridService gridService;

        public SpriteFactory
            (
            IContentRepository contentRepository,
            IGridService gridService
            )
        {
            this.contentRepository = contentRepository;
            this.gridService = gridService;
        }

        public Bullet CreateBullet(string bulletId, Point point, Direction direction)
        {
            var bulletMaster = contentRepository.Bullets.First(b => string.Equals(b.BulletId, bulletId)).Bullet;

            var masterJson = JsonConvert.SerializeObject(bulletMaster);
            var bullet = JsonConvert.DeserializeObject<Bullet>(masterJson);

            bullet.SpriteId = Guid.NewGuid().ToString();
            bullet.X = point.X;
            bullet.Y = point.Y;
            bullet.CurrentDirection = direction;

            gridService.AddSpriteToGrid(contentRepository.CurrentStage.Grids, GridContainerSpriteType.Bullet, bullet);

            return bullet;
        }
    }
}
