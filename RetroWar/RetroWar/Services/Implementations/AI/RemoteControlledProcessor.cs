using Microsoft.Xna.Framework.Input;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Services.Interfaces.AI;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.UserInterface;

namespace RetroWar.Services.Implementations.AI
{
    public class RemoteControlledProcessor : IBehaviorProcessor
    {
        private readonly IContentRepository contentRepository;
        private readonly IGridHandler gridHandler;
        private readonly IInputService inputService;

        public RemoteControlledProcessor
            (
                IContentRepository contentRepository,
                IGridHandler gridHandler,
                IInputService inputService
            )
        {
            this.contentRepository = contentRepository;
            this.gridHandler = gridHandler;
            this.inputService = inputService;
        }

        public bool ProcessBehavior(EnemyVehicle enemy, float deltaTime)
        {
            if (enemy.Behavior != AIBehavior.RemoteControlled)
            {
                return false;
            }

            enemy.OldX = enemy.X;
            enemy.OldY = enemy.Y;

            if (enemy.CurrentActions.Contains(Action.Destroyed))
            {
                enemy.FallSum += System.Math.Min(enemy.FallRate * deltaTime, 10);

                enemy.Y += enemy.FallSum;

                gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, enemy);

                return true;
            }

            if (inputService.KeyPressed(Keys.Left) && !enemy.CurrentActions.Contains(Action.Destroyed))
            {
                enemy.deltaX -= enemy.VehicleSpeed * deltaTime;
                enemy.CurrentDirection = Direction.Left;
            }

            if (inputService.KeyPressed(Keys.Right) && !enemy.CurrentActions.Contains(Action.Destroyed))
            {
                enemy.deltaX += enemy.VehicleSpeed * deltaTime;
                enemy.CurrentDirection = Direction.Right;
            }

            if (inputService.KeyPressed(Keys.Up) && !enemy.CurrentActions.Contains(Action.Destroyed))
            {
                if (enemy.FallSum == 0 && enemy.IsJumping == false)
                {
                    enemy.FallSum = -5;
                    enemy.IsJumping = true;
                }
            }

            enemy.FallSum += System.Math.Min(enemy.FallRate * deltaTime, 10);
            enemy.FallSum = System.Math.Min(enemy.FallSum, 15);
            enemy.Y += enemy.FallSum;

            enemy.X += enemy.deltaX;
            enemy.deltaX = 0;

            gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, enemy);

            return true;
        }
    }
}
