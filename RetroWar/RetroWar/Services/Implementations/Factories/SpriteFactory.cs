using Newtonsoft.Json;
using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Illusions;
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

        public Bullet CreateBullet(string bulletId, Point point, Direction direction, DamageDiscrimination damageDiscrimination)
        {
            var bulletMaster = contentRepository.Bullets.First(b => string.Equals(b.BulletId, bulletId)).Bullet;

            var masterJson = JsonConvert.SerializeObject(bulletMaster);
            var bullet = JsonConvert.DeserializeObject<Bullet>(masterJson);

            bullet.SpriteId = $"{Guid.NewGuid().ToString()}_{bulletId}";
            bullet.X = point.X;
            bullet.Y = point.Y;
            bullet.CurrentDirection = direction;
            bullet.DamageDiscrimination = damageDiscrimination;

            gridService.AddSpriteToGrid(contentRepository.CurrentStage.Grids, bullet);

            return bullet;
        }

        public Illusion CreateIllusion(string illusionId, Point point)
        {
            var illusionMaster = contentRepository.Illusions.First(i => string.Equals(i.IllusionId, illusionId)).Illusion;

            var masterJson = JsonConvert.SerializeObject(illusionMaster);
            var illusion = JsonConvert.DeserializeObject<Illusion>(masterJson);

            illusion.SpriteId = $"{Guid.NewGuid().ToString()}_{illusionId}";
            illusion.X = point.X;
            illusion.Y = point.Y;

            gridService.AddSpriteToGrid(contentRepository.CurrentStage.Grids, illusion);

            return illusion;
        }
    }
}
