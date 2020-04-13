using RetroWar.Models.Sprites.Actions;
using System.Collections.Generic;

namespace RetroWar.Models.Sprites
{
    public class Sprite
    {
        public float X;
        public float Y;
        public float OldX;
        public float OldY;
        public int previousXGridPoint;
        public int previousYGridPoint;
        public float deltaX;
        public float deltaY;
        public string SpriteId;
        public string Name;
        public Dictionary<Action, int> CurrentActionSequence;
        public Dictionary<Action, float> ActionTickAccumulation;
        public Direction CurrentDirection;
        public SortedSet<Action> CurrentActions;
        public string ActionDataSetId;
        public IEnumerable<ActionData> ActionDataSet;
    }
}
