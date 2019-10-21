using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class Sprite
    {
        public float X;
        public float Y;
        public int CurrentSequence;
        public Action CurrentAction;
        public string ActionDataSetId;
        public IEnumerable<ActionData> ActionDataSet;
    }
}
