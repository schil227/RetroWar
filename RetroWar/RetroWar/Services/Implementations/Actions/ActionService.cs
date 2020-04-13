using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Factories;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Repositories;
using System.Linq;
using Action = RetroWar.Models.Sprites.Actions.Action;

namespace RetroWar.Services.Implementations.Actions
{
    public class ActionService : IActionService
    {
        private readonly ISpriteHelper spriteHelper;
        private readonly IContentRepository contentRepository;
        private readonly ISpriteFactory spriteFactory;
        private readonly IGridHandler gridHandler;
        private readonly IBulletHelper bulletHelper;

        public ActionService(
            ISpriteHelper spriteHelper,
            IContentRepository contentRepository,
            ISpriteFactory spriteFactory,
            IGridHandler gridHandler,
            IBulletHelper bulletHelper
            )
        {
            this.spriteHelper = spriteHelper;
            this.contentRepository = contentRepository;
            this.spriteFactory = spriteFactory;
            this.gridHandler = gridHandler;
            this.bulletHelper = bulletHelper;
        }

        public void ProcessActionEvent(Sprite sprite, Action action)
        {
            var actionData = spriteHelper.GetCurrentActionData(sprite).First(ad => ad.Action == action);

            if (actionData.SequencesToTriggerOn.Contains(sprite.CurrentActionSequence[actionData.Action]))
            {
                switch (actionData.Action)
                {
                    case Action.FireStandard:
                        {
                            bulletHelper.FireBullet(sprite as Vehicle, FiringMode.Default);
                            break;
                        }
                    case Action.FireCharged:
                        {
                            bulletHelper.FireBullet(sprite as Vehicle, FiringMode.Charged);
                            break;
                        }
                    case Action.Destroyed:
                        {
                            gridHandler.RemoveSpriteFromGrid(contentRepository.CurrentStage.Grids, sprite);

                            if (sprite is Vehicle)
                            {
                                var spawnPoint = new Point
                                {
                                    X = sprite.X,
                                    Y = sprite.Y
                                };

                                spriteFactory.CreateIllusion("Explosion", spawnPoint);
                            }

                            break;
                        }
                }
            }
        }

        public void RemoveAction(Sprite sprite, Action action)
        {
            sprite.CurrentActionSequence[action] = 0;
            sprite.CurrentActions.Remove(action);
        }

        public void SetAction(Sprite sprite, Action action, Action? actionToRemove)
        {
            sprite.CurrentActionSequence[action] = 0;
            sprite.ActionTickAccumulation[action] = 0;
            sprite.CurrentActions.Add(action);

            if (actionToRemove != null)
            {
                sprite.CurrentActions.Remove((Action)actionToRemove);
            }

            ProcessActionEvent(sprite, action);
        }
    }
}
