using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Illusions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.Updaters;
using RetroWar.Services.Interfaces.UserInterface;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Updaters
{
    public class IllusionUpdater : ISpriteUpdater
    {

        private readonly IContentRepository contentRepository;
        private readonly IScreenService screenService;
        private readonly IGridHandler gridHandler;

        public IllusionUpdater(
                IContentRepository contentRepository,
                IScreenService screenService,
                IGridHandler gridHandler
            )
        {
            this.contentRepository = contentRepository;
            this.screenService = screenService;
            this.gridHandler = gridHandler;
        }

        public bool UpdateSprite(Sprite sprite, float deltaTime, Dictionary<string, string> processedSprites)
        {
            Illusion illusion = null;

            if (sprite is Illusion)
            {
                illusion = (Illusion)sprite;
            }
            else
            {
                return false;
            }

            var screen = contentRepository.Screen;
            var stage = contentRepository.CurrentStage;

            illusion.OldX = illusion.X;
            illusion.OldY = illusion.Y;

            if (!screenService.IsOnScreen(screen, illusion))
            {
                gridHandler.RemoveSpriteFromGrid(stage.Grids, illusion);
                processedSprites.Add(illusion.SpriteId, "Processed");
                return true;
            }

            if (!illusion.SubjectToGravity)
            {
                processedSprites.Add(illusion.SpriteId, "Processed");
                return true;
            }

            illusion.FallSum += Math.Min(illusion.FallRate * deltaTime, 10);
            illusion.Y += illusion.FallSum;

            gridHandler.MoveSprite(stage.Grids, illusion);

            processedSprites.Add(illusion.SpriteId, "Processed");
            return true;
        }
    }
}
