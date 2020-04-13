using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Vehicles.Vehicles.EnemyVehicle;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Repositories;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class BulletEnemyVehicleCollisionResolver : ICollisionResolver
    {
        private readonly IGridHandler gridHandler;
        private readonly IActionService actionService;
        public readonly IContentRepository contentRepository;

        public BulletEnemyVehicleCollisionResolver
            (
                IGridHandler gridHandler,
                IActionService actionService,
                IContentRepository contentRepository
            )
        {
            this.gridHandler = gridHandler;
            this.actionService = actionService;
            this.contentRepository = contentRepository;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            EnemyVehicle enemyTank = null;
            Bullet bullet = null;

            if (normal is PlayerVehicle && based is Bullet)
            {
                enemyTank = (EnemyVehicle)normal;
                bullet = (Bullet)based;
            }
            else if (normal is Bullet && based is EnemyVehicle)
            {
                enemyTank = (EnemyVehicle)based;
                bullet = (Bullet)normal;
            }
            else
            {
                return false;
            }

            if (bullet.DamageDiscrimination != DamageDiscrimination.DamagesEnemy && bullet.DamageDiscrimination != DamageDiscrimination.DamagesAll)
            {
                return true;
            }

            enemyTank.Health -= bullet.Damage;

            if (enemyTank.Health <= 0)
            {
                actionService.SetAction(enemyTank, Action.Destroyed);
            }

            gridHandler.RemoveSpriteFromGrid(contentRepository.CurrentStage.Grids, bullet);

            return true;
        }
    }
}
