using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.Updaters;
using RetroWar.Services.Interfaces.UserInterface;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Updaters
{
    public class BulletUpdater : ISpriteUpdater
    {
        private readonly IContentRepository contentRepository;
        private readonly IBulletHelper bulletHelper;
        private readonly IScreenService screenService;
        private readonly IGridHandler gridHandler;

        public BulletUpdater(
                IContentRepository contentRepository,
                IBulletHelper bulletHelper,
                IScreenService screenService,
                IGridHandler gridHandler
            )
        {
            this.contentRepository = contentRepository;
            this.bulletHelper = bulletHelper;
            this.screenService = screenService;
            this.gridHandler = gridHandler;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            Bullet bullet = null;

            if (sprite is Bullet)
            {
                bullet = (Bullet)sprite;
            }
            else
            {
                return false;
            }

            var stage = contentRepository.CurrentStage;
            var screen = contentRepository.Screen;

            var oldX = (int)bullet.X;
            var oldY = (int)bullet.Y;

            bullet.TotalTime += deltaTime;

            var newPoint = bulletHelper.FindNextPointInTrajectory(bullet, deltaTime);

            bullet.X = newPoint.X;
            bullet.Y = newPoint.Y;

            if (!screenService.IsOnScreen(screen, bullet))
            {
                gridHandler.RemoveSpriteFromGrid(stage.Grids, bullet, oldX, oldY);
                processedSprites.Add(bullet.SpriteId, "Processed");
                return true;
            }

            gridHandler.MoveSprite(stage.Grids, bullet, oldX, oldY);

            processedSprites.Add(bullet.SpriteId, "Processed");

            return true;
        }
    }
}
