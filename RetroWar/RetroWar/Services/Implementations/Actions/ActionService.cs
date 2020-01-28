using RetroWar.Models.Common;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
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

        public ActionService(
            ISpriteHelper spriteHelper,
            IContentRepository contentRepository,
            ISpriteFactory spriteFactory,
            IGridHandler gridHandler
            )
        {
            this.spriteHelper = spriteHelper;
            this.contentRepository = contentRepository;
            this.spriteFactory = spriteFactory;
            this.gridHandler = gridHandler;
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
                            var vehicle = sprite as Vehicle;

                            var firingData = vehicle.FiringData[vehicle.CurrentFiringMode];

                            var directionOffset = sprite.CurrentDirection == Direction.Right ? firingData.XSpawningOffset : firingData.XSpawningOffset * -1;

                            var spawnPoint = new Point
                            {
                                X = sprite.X + directionOffset,
                                Y = sprite.Y + firingData.YSpawningOffset
                            };

                            var damageType = (vehicle is PlayerVehicle ? DamageDiscrimination.DamagesEnemy : DamageDiscrimination.DamagesPlayer);

                            spriteFactory.CreateBullet(firingData.BulletId, spawnPoint, sprite.CurrentDirection, damageType);
                            break;
                        }
                    case Action.Destroyed:
                        {
                            gridHandler.RemoveSpriteFromGrid(contentRepository.CurrentStage.Grids, sprite, (int)sprite.X, (int)sprite.Y);

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

        public void SetAction(Sprite sprite, Action action)
        {
            sprite.CurrentSequence = 0;
            sprite.TickAccumulation = 0;
            sprite.CurrentAction = action;

            ProcessActionEvent(sprite);
        }
    }
}
