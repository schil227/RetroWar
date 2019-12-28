using RetroWar.Models.Sprites.Actions;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class Sprite
    {
        public float X;
        public float Y;
        public float deltaX;
        public float deltaY;
        public string SpriteId;
        public string Name;
        public int CurrentSequence;
        public float TickAccumulation;
        public Direction CurrentDirection;
        public Action CurrentAction;
        public string ActionDataSetId;
        public IEnumerable<ActionData> ActionDataSet;
    }
}
