using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Services.Interfaces.AI;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Repositories;

namespace RetroWar.Services.Implementations.AI
{
    public class MoveLeftAndRightProcessor : IBehaviorProcessor
    {
        private readonly IContentRepository contentRepository;
        private readonly IGridHandler gridHandler;

        public MoveLeftAndRightProcessor
            (
                IContentRepository contentRepository,
                IGridHandler gridHandler
            )
        {
            this.contentRepository = contentRepository;
            this.gridHandler = gridHandler;
        }

        public bool ProcessBehavior(EnemyVehicle enemy, float deltaTime)
        {
            if (enemy.Behavior != AIBehavior.MoveLeftAndRight)
            {
                return false;
            }

            enemy.OldX = enemy.X;
            enemy.OldY = enemy.Y;

            if (!enemy.CurrentActions.Contains(Models.Sprites.Actions.Action.Destroyed))
            {
                var directionVector = enemy.CurrentDirection == Models.Sprites.Direction.Left ? -1 : 1;

                enemy.X += enemy.VehicleSpeed * deltaTime * directionVector;
            }

            enemy.FallSum += System.Math.Min(enemy.FallRate * deltaTime, 10);
            enemy.Y += enemy.FallSum;

            gridHandler.MoveSprite(contentRepository.CurrentStage.Grids, enemy);

            return true;
        }
    }
}
