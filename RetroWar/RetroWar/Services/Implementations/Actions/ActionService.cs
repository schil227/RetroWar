using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Services.Interfaces.Actions;
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

        public ActionService(
            ISpriteHelper spriteHelper,
            IContentRepository contentRepository,
            ISpriteFactory spriteFactory
            )
        {
            this.spriteHelper = spriteHelper;
            this.contentRepository = contentRepository;
            this.spriteFactory = spriteFactory;
        }

        public void ProcessActionEvent(Sprite sprite)
        {
            var currentActionData = spriteHelper.GetCurrentActionData(sprite);

            if (currentActionData.SequencesToTriggerOn.Contains(sprite.CurrentSequence))
            {
                switch (sprite.CurrentAction)
                {
                    case Action.FireStandard:
                        {
                            // This bullet spawning metadata needs to be wrapped in an object and (probably)
                            // added to a sub-class of sprite that supports firing
                            var directionOffset = sprite.CurrentDirection == Direction.Right ? 18 : -18;

                            var spawnPoint = new Point
                            {
                                X = sprite.X + directionOffset,
                                Y = sprite.Y + 6
                            };

                            spriteFactory.CreateBullet("StandardBullet", spawnPoint, sprite.CurrentDirection);
                            break;
                        }
                }
            }
        }

        public void SetAction(Sprite sprite, Action action)
        {
            sprite.CurrentSequence = 0;
            sprite.TickAccumulation = 0;
            sprite.CurrentAction = action;

            ProcessActionEvent(sprite);
        }
    }
}
