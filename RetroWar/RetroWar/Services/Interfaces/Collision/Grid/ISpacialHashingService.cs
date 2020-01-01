using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using System;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Collision.Grid
{
    public interface ISpacialHashingService
    {
        void AddSpriteToGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY);
        void RemoveSpriteFromGrid(Dictionary<Tuple<int, int>, GridContainer> gridHash, GridContainerSpriteType spriteType, Sprite sprite, int gridX, int gridY);
        GridContainer GetGridContainer(Dictionary<Tuple<int, int>, GridContainer> gridHash, int gridX, int gridY);
    }
}
