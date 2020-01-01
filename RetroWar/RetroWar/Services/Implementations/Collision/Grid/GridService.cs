using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision.Grid
{
    public class GridService : IGridService
    {
        private readonly ISpacialHashingService spacialHashingService;
        private readonly ISpriteHelper spriteHelper;

        public GridService(
            ISpacialHashingService spacialHashingService,
            ISpriteHelper spriteHelper
            )
        {
            this.spacialHashingService = spacialHashingService;
            this.spriteHelper = spriteHelper;
        }

        public void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite)
        {
            var maximumPoint = spriteHelper.GetMaximumPoints(sprite);

            int startX = (int)(sprite.X / 32);
            int startY = (int)(sprite.Y / 32);

            int endX = maximumPoint.X / 32;
            int endY = maximumPoint.Y / 32;

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    spacialHashingService.AddSpriteToGrid(gridHash, spriteType, sprite, i, j);
                }
            }
        }

        public IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY)
        {
            int startX = x / 32;
            int startY = y / 32;

            int endX = maxX / 32;
            int endY = maxY / 32;

            var grids = new List<GridContainer>();

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    grids.Add(spacialHashingService.GetGridContainer(gridHash, i, j));
                }
            }

            return grids;
        }

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int oldX, int oldY)
        {
            var maximumPoint = spriteHelper.GetMaximumPoints(sprite);

            int startX = oldX / 32;
            int startY = oldY / 32;

            int endX = maximumPoint.X / 32;
            int endY = maximumPoint.Y / 32;

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    spacialHashingService.RemoveSpriteFromGrid(gridHash, spriteType, sprite, i, j);
                }
            }
        }
    }
}
