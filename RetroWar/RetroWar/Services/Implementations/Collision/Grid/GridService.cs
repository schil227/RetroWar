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

        public void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite)
        {
            var maximumPoint = spriteHelper.GetMaximumPoints(sprite, (int)sprite.X, (int)sprite.Y);

            int startX = GetBoxCoord((int)sprite.X);
            int startY = GetBoxCoord((int)sprite.Y);

            int endX = GetBoxCoord(maximumPoint.X);
            int endY = GetBoxCoord(maximumPoint.Y);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    spacialHashingService.AddSpriteToGrid(gridHash, sprite, i, j);
                }
            }
        }

        public IEnumerable<GridContainer> GetGridsFromPoints(Dictionary<Tuple<int, int>, GridContainer> gridHash, int x, int y, int maxX, int maxY)
        {
            int startX = GetBoxCoord(x);
            int startY = GetBoxCoord(y);

            int endX = GetBoxCoord(maxX);
            int endY = GetBoxCoord(maxY);

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

        public void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, Sprite sprite, int oldX, int oldY)
        {
            var maximumPoint = spriteHelper.GetMaximumPoints(sprite, oldX, oldY);

            int startX = GetBoxCoord(oldX);
            int startY = GetBoxCoord(oldY);

            int endX = GetBoxCoord(maximumPoint.X);
            int endY = GetBoxCoord(maximumPoint.Y);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    spacialHashingService.RemoveSpriteFromGrid(gridHash, sprite, i, j);
                }
            }
        }

        private int GetBoxCoord(int position)
        {
            // negative values need to be pushed to the previous box, 
            // since a position of, say -5 would otherwise put them in the wrong box
            // -5/32  => box 0
            // subtracting 1 when the postion is negative corrects this:
            // (-5/32) - 1 => box -1
            // box:... -2          -1          0        1         2  ...
            // posn:   [-64, -32]  [-32, -0]  [0, 32]   [32, 64]  [64, 96]
            return position > 0 ? position / 32 : (position / 32) - 1;
        }
    }
}
