﻿using Microsoft.Extensions.DependencyInjection;
using RetroWar.Services.Implementations.Actions;
using RetroWar.Services.Implementations.Collision;
using RetroWar.Services.Implementations.Collision.Grid;
using RetroWar.Services.Implementations.Collision.Resolvers;
using RetroWar.Services.Implementations.Factories;
using RetroWar.Services.Implementations.Helpers;
using RetroWar.Services.Implementations.Helpers.Model;
using RetroWar.Services.Implementations.Loaders;
using RetroWar.Services.Implementations.Repositories;
using RetroWar.Services.Implementations.Updaters;
using RetroWar.Services.Implementations.UserInterface;
using RetroWar.Services.Interfaces.Actions;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Grid;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Factories;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Helpers.Model;
using RetroWar.Services.Interfaces.Loaders;
using RetroWar.Services.Interfaces.Repositories;
using RetroWar.Services.Interfaces.Updaters;
using RetroWar.Services.Interfaces.UserInterface;
using System.Collections.Generic;

namespace RetroWar
{
    public static class CompositionRoot
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<MainGame>();

            // Data
            services.AddSingleton<IContentRepository, ContentRepository>();

            // Actions
            services.AddSingleton<IActionService, ActionService>();
            services.AddSingleton<ISequenceService, SequenceService>();

            // Factories
            services.AddSingleton<ISpriteFactory, SpriteFactory>();

            // Helpers
            services.AddSingleton<IBulletHelper, BulletHelper>();
            services.AddSingleton<IStreamReader, StreamReader>();
            services.AddSingleton<ISpriteHelper, SpriteHelper>();

            // Loaders
            services.AddSingleton<IActionDataLoader, ActionDataLoader>();
            services.AddSingleton<IPlayerVehicleLoader, PlayerVehicleLoader>();
            services.AddSingleton<IEnemyVehicleLoader, EnemyVehicleLoader>();
            services.AddSingleton<ITextureLoader, TextureLoader>();
            services.AddSingleton<ITileLoader, TileLoader>();
            services.AddSingleton<IBulletLoader, BulletLoader>();
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
            services.AddSingleton<VehicleTileCollisionResolver>();
            services.AddSingleton<BulletTileCollisionResolver>();

            services.AddSingleton<IEnumerable<ICollisionResolver>>(
                provider => new List<ICollisionResolver>
                {
                    provider.GetService<VehicleTileCollisionResolver>(),
                    provider.GetService<BulletTileCollisionResolver>()
                });

            services.AddSingleton<ICollisionResolver, CompositeCollisionResolver>();

            // Sprite Updaters

            services.AddSingleton<BulletUpdater>();
            services.AddSingleton<PlayerUpdater>();

            services.AddSingleton<IEnumerable<ISpriteUpdater>>(
                provider => new List<ISpriteUpdater>
                {
                    provider.GetService<BulletUpdater>(),
                    provider.GetService<PlayerUpdater>()
                });

            services.AddSingleton<ISpriteUpdater, SpriteUpdaterComposite>();

            // Grid
            services.AddSingleton<IGridHandler, GridHandler>();
            services.AddSingleton<IGridService, GridService>();
            services.AddSingleton<ISpacialHashingService, SpacialHashingService>();
        }
    }
}
