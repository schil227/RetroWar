using RetroWar.Models.Collisions;
using RetroWar.Models.Collisions.Grid;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Bullets;
using RetroWar.Models.Sprites.Tiles;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Repositories;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class BulletTileCollisionResolver : ICollisionResolver
    {
        public readonly IGridHandler gridHandler;
        public readonly IContentRepository contentRepository;

        public BulletTileCollisionResolver
            (
                IGridHandler gridHandler,
                IContentRepository contentRepository
            )
        {
            this.gridHandler = gridHandler;
            this.contentRepository = contentRepository;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Tile tile = null;
            Bullet bullet = null;

            if (normal is Tile && based is Bullet)
            {
                tile = (Tile)normal;
                bullet = (Bullet)based;
            }
            else if (normal is Bullet && based is Tile)
            {
                tile = (Tile)based;
                bullet = (Bullet)normal;
            }
            else
            {
                return false;
            }

            gridHandler.RemoveSpriteFromGrid(contentRepository.CurrentStage.Grids, bullet, GridContainerSpriteType.Bullet, (int)bullet.X, (int)bullet.Y);

            return true;
        }
    }
}
