using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class Sprite
    {
        public float X;
        public float Y;
        public float deltaX;
        public float deltaY;
        public int CurrentSequence;
        public Action CurrentAction;
        public string ActionDataSetId;
        public IEnumerable<ActionData> ActionDataSet;
    }
}
