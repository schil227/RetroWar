using Microsoft.Extensions.DependencyInjection;
using RetroWar.Services.Implementations.Helpers;
using RetroWar.Services.Implementations.Loaders;
using RetroWar.Services.Interfaces.Helpers;
using RetroWar.Services.Interfaces.Loaders;

namespace RetroWar
{
    public static class CompositionRoot
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<MainGame>();
            services.AddSingleton<IStreamReader, StreamReader>();
            services.AddSingleton<IActionDataLoader, ActionDataLoader>();
            services.AddSingleton<ISpriteLoader, SpriteLoader>();
            services.AddSingleton<ITextureLoader, TextureLoader>();
        }
    }
}
