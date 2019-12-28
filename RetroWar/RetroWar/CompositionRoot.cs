using Microsoft.Extensions.DependencyInjection;
using RetroWar.Services.Implementations.Actions;
using RetroWar.Services.Implementations.Collision;
using RetroWar.Services.Implementations.Collision.Grid;
using RetroWar.Services.Implementations.Collision.Resolvers;
using RetroWar.Services.Implementations.Events;
using RetroWar.Services.Implementations.Helpers;
using RetroWar.Services.Implementations.Helpers.Model;
using RetroWar.Services.Implementations.Loaders;
using RetroWar.Services.Implementations.UserInterface;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Events;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;
using RetroWar.Services.Interfaces.UserInterface;
using System.Collections.Generic;

namespace RetroWar
{
    public static class CompositionRoot
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<MainGame>();

            // Actions
            services.AddSingleton<IActionService, ActionService>();
            services.AddSingleton<ISequenceService, SequenceService>();

            // Helpers
            services.AddSingleton<IStreamReader, StreamReader>();
            services.AddSingleton<ISpriteHelper, SpriteHelper>();

            // Loaders
            services.AddSingleton<IActionDataLoader, ActionDataLoader>();
            services.AddSingleton<ISpriteLoader, SpriteLoader>();
            services.AddSingleton<ITextureLoader, TextureLoader>();
            services.AddSingleton<ITileLoader, TileLoader>();
            services.AddSingleton<IContentLoader, ContentLoader>();

            // User Interface
            services.AddSingleton<IScreenService, ScreenService>();
            services.AddSingleton<IDrawService, DrawService>();

            // Collision
            services.AddSingleton<ICollisionFinder, CollisionFinder>();
            services.AddSingleton<ICollisionService, CollisionService>();
            services.AddSingleton<IFoundCollisionFilter, FoundCollisionFilter>();
            services.AddSingleton<IMultiPointCollisionResolver, MultiPointCollisionResolver>();

            // Collision Resolvers
            services.AddSingleton<ICollisionResolver, CollisionResolver>();

            // Grid
            services.AddSingleton<IGridHandler, GridHandler>();
            services.AddSingleton<IGridService, GridService>();
            services.AddSingleton<ISpacialHashingService, SpacialHashingService>();

            // Events
            services.AddSingleton<IEventHandler, EventHandlerComposite>();
            services.AddSingleton<SpawnSpriteEventHandler>();
            services.AddSingleton<IEnumerable<IEventHandler>>(provider =>
                new List<IEventHandler>
                {
                    provider.GetService<SpawnSpriteEventHandler>()
                }
                );
        }
    }
}
