using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Sprites;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Helpers.Model;
using System.Collections.Generic;
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

        public void IncrementSequence(Sprite sprite, Action action)
        {
            var actionData = spriteHelper.GetCurrentActionData(sprite).First(ad => ad.Action == action);

            sprite.CurrentActionSequence[action]++;

            if (sprite.CurrentActionSequence[action] >= actionData.TotalSequences)
            {
                sprite.CurrentActionSequence[action] = 0;

                if (!actionData.IsContinuous)
                {
                    switch (action)
                    {
                        case Action.FireStandard:
                            {
                                if (sprite is PlayerVehicle && Keyboard.GetState().IsKeyDown(Keys.K))
                                {
                                    actionService.SetAction(sprite, Action.Charging, action);
                                    break;
                                }
                                else
                                {
                                    actionService.SetAction(sprite, Action.Armed, action);
                                }

                                break;
                            }
                        case Action.Move:
                            {
                                actionService.SetAction(sprite, Action.Stationary, action);
                                break;
                            }
                        case Action.Charging:
                            {
                                actionService.SetAction(sprite, Action.Charged, action);
                                break;
                            }
                        case Action.FireCharged:
                            {
                                actionService.SetAction(sprite, Action.Armed, action);
                                break;
                            }
                        default:
                            {
                                sprite.CurrentActions.Remove(action);
                                break;
                            }
                    }

                    return;
                }
            }

            actionService.ProcessActionEvent(sprite, action);
        }

        public void UpdateActionSequence(Sprite sprite, float deltaTimeTick)
        {
            foreach (var action in new List<Action>(sprite.CurrentActions))
            {
                var currentAction = sprite.ActionDataSet.First(a => a.Action == action);
                // gotta rebuild this whole thing; tick accumulation is relative to the current action
                sprite.ActionTickAccumulation[action] += deltaTimeTick;

                var currentTickDuration = currentAction.SequenceDurations.ElementAt(sprite.CurrentActionSequence[action]);

                while (sprite.ActionTickAccumulation[action] > currentTickDuration)
                {
                    IncrementSequence(sprite, action);
                    sprite.ActionTickAccumulation[action] = sprite.ActionTickAccumulation[action] - currentTickDuration;
                }
            }
        }
    }
}
