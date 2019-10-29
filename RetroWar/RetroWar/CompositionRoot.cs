using Microsoft.Extensions.DependencyInjection;
using RetroWar.Services.Implementations.Collision;
using RetroWar.Services.Implementations.Collision.Resolvers;
using RetroWar.Services.Implementations.Helpers;
using RetroWar.Services.Implementations.Helpers.Model;
using RetroWar.Services.Implementations.Loaders;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;

namespace RetroWar
{
    public static class CompositionRoot
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<MainGame>();

            // Helpers
            services.AddSingleton<IStreamReader, StreamReader>();
            services.AddSingleton<ISpriteHelper, SpriteHelper>();

            // Loaders
            services.AddSingleton<IActionDataLoader, ActionDataLoader>();
            services.AddSingleton<ISpriteLoader, SpriteLoader>();
            services.AddSingleton<ITextureLoader, TextureLoader>();
            services.AddSingleton<ITileLoader, TileLoader>();

            // Collision
            services.AddSingleton<ICollisionFinder, CollisionFinder>();
            services.AddSingleton<ICollisionService, CollisionService>();
            services.AddSingleton<IFoundCollisionFilter, FoundCollisionFilter>();
            services.AddSingleton<IMultiPointCollisionResolver, MultiPointCollisionResolver>();

            // Collision Resolvers
            services.AddSingleton<ICollisionResolver, CollisionResolver>();
        }
    }
}
