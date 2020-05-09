using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Repositories;
using System.Collections.Generic;

namespace RetroWar.Services.Implementations.Collision
{
    public class CollisionService : ICollisionService
    {
        private readonly ICollisionResolver collisionResolver;
        private readonly ICollisionChecker collisionChecker;
        private readonly ICollisionFinder collisionFinder;
        private readonly IGridHandler gridHandler;
        private readonly IContentRepository contentRepository;
        private readonly ISpriteHelper spriteHelper;

        public CollisionService
            (
                ICollisionResolver collisionResolver,
                ICollisionChecker collisionChecker,
                ICollisionFinder collisionFinder,
                IGridHandler gridHandler,
                IContentRepository contentRepository,
                ISpriteHelper spriteHelper
            )
        {
            this.collisionResolver = collisionResolver;
            this.collisionChecker = collisionChecker;
            this.collisionFinder = collisionFinder;
            this.gridHandler = gridHandler;
            this.contentRepository = contentRepository;
            this.spriteHelper = spriteHelper;
        }

        public bool HandleCollision(Sprite normal, Sprite based, float deltaTime)
        {
            if (!collisionChecker.AreColliding(normal, based))
            {
                if (normal is Vehicle && based is Vehicle)
                {
                    ((Vehicle)normal).StickyCollisionData.Remove(based.SpriteId);
                    ((Vehicle)based).StickyCollisionData.Remove(normal.SpriteId);
                }

                return false;
            }

            var collision = collisionFinder.FindCollisionResolutionFace(normal, based, deltaTime);

            if (collisionResolver.ResolveCollision(normal, based, collision))
            {

                if (normal is Vehicle normalVehicle)
                {
                    UpdateNearbyVehicleCollisionSticky(normalVehicle);
                }

                return true;
            }

            return false;
        }

        public void UpdateNearbyVehicleCollisionSticky(Vehicle normal)
        {
            var maxPoint = spriteHelper.GetMaximumHitboxPoints(normal);

            var boxes = gridHandler.GetGridsFromPoints(contentRepository.CurrentStage.Grids, (int)normal.X - 1, (int)normal.Y - 1, maxPoint.X + 1, maxPoint.Y + 1);

            var checkedVehicles = new HashSet<string>();

            foreach (var box in boxes)
            {
                var vehicles = new List<Vehicle>();

                vehicles.AddRange(box.EnemyVehicles.Values);

                if (box.playerTank != null)
                {
                    vehicles.Add(box.playerTank);
                }

                foreach (var based in vehicles)
                {
                    if (checkedVehicles.Contains(based.SpriteId) ||
                        normal.SpriteId == based.SpriteId ||
                        normal.StickyCollisionData.ContainsKey(based.SpriteId) ||
                        !collisionChecker.AreColliding(normal, based)
                        )
                    {
                        continue;
                    }

                    collisionFinder.SetStickyFace(normal, based);

                    checkedVehicles.Add(based.SpriteId);
                }
            }
        }
    }
}
