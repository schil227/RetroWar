using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using System.Collections.Generic;

namespace RetroWar.Services.Interfaces.Helpers.Model
{
    public interface IGridContainerHelper
    {
        HashSet<Sprite> GetActionableGridSprites(GridContainer box);
    }
}
