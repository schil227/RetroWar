using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Linq;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar.Services.Implementations.Actions
{
    public class SequenceService : ISequenceService
    {
        private readonly ISpriteHelper spriteHelper;
        private readonly IActionService actionService;

        public SequenceService(
            ISpriteHelper spriteHelper,
            IActionService actionService
            )
        {
            this.spriteHelper = spriteHelper;
            this.actionService = actionService;
        }

        public void IncrementSequence(Sprite sprite)
        {
            sprite.CurrentSequence++;
            var currentAction = sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction);

            if (sprite.CurrentSequence >= currentAction.TotalSequences)
            {
                sprite.CurrentSequence = 0;

                if (!currentAction.IsContinuous)
                {
                    actionService.SetAction(sprite, Action.Idle);
                    return;
                }
            }

            actionService.ProcessActionEvent(sprite);
        }

        public void UpdateActionSequence(Sprite sprite, float deltaTimeTick)
        {
            var currentAction = sprite.ActionDataSet.First(a => a.Action == sprite.CurrentAction);
            sprite.TickAccumulation += deltaTimeTick;

            var currentTickDuration = currentAction.SequenceDurations.ElementAt(sprite.CurrentSequence);

            while (sprite.TickAccumulation > currentTickDuration)
            {
                IncrementSequence(sprite);
                sprite.TickAccumulation = sprite.TickAccumulation - currentTickDuration;
            }
        }
    }
}
