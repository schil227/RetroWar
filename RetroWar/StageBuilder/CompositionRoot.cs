using Microsoft.Extensions.DependencyInjection;
using StageBuilder.Services.Implementations.Building;
using StageBuilder.Services.Implementations.Exporters;
using StageBuilder.Services.Implementations.UI;
using StageBuilder.Services.Implementations.Updaters;
using StageBuilder.Services.Interfaces.Building;
using StageBuilder.Services.Interfaces.Exporters;
using StageBuilder.Services.Interfaces.UI;
using StageBuilder.Services.Interfaces.Updaters;

namespace StageBuilder
{
    public static class CompositionRoot
    {
        public static void RegisterStageBuilderServices(this IServiceCollection services)
        {
            services.AddSingleton<IStageBuilderDrawingService, StageBuilderDrawingService>();
            services.AddSingleton<ICursorUpdater, CursorUpdater>();
            services.AddSingleton<IBuilderService, BuilderService>();
            services.AddSingleton<IStageExporter, StageExporter>();

            services.AddSingleton<StageBuilder>();
        }
    }
}
