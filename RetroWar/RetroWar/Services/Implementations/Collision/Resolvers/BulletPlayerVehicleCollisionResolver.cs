using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Actions;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Vehicles.Vehicles.PlayerVehicle;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Repositories;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class BulletPlayerVehicleCollisionResolver : ICollisionResolver
    {
        private readonly IGridHandler gridHandler;
        private readonly IActionService actionService;
        public readonly IContentRepository contentRepository;

        public BulletPlayerVehicleCollisionResolver
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
            PlayerVehicle playerTank = null;
            Bullet bullet = null;

            if (normal is PlayerVehicle && based is Bullet)
            {
                playerTank = (PlayerVehicle)normal;
                bullet = (Bullet)based;
            }
            else if (normal is Bullet && based is PlayerVehicle)
            {
                playerTank = (PlayerVehicle)based;
                bullet = (Bullet)normal;
            }
            else
            {
                return false;
            }

            if (bullet.DamageDiscrimination != DamageDiscrimination.DamagesPlayer && bullet.DamageDiscrimination != DamageDiscrimination.DamagesAll)
            {
                return true;
            }

            playerTank.Health -= bullet.Damage;

            if (playerTank.Health <= 0)
            {
                actionService.SetAction(playerTank, Action.Destroyed);
            }

            gridHandler.RemoveSpriteFromGrid(contentRepository.CurrentStage.Grids, bullet);

            return true;
        }
    }
}
