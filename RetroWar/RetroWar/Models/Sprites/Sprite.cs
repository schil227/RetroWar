using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class Sprite
    {
        public int X;
        public int Y;
        public int CurrentSequence;
        public Action CurrentAction;
        public int ActionDataSetId;
        public IEnumerable<ActionData> ActionDataSet;
    }
}
