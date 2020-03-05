using Microsoft.Extensions.DependencyInjection;
using RetroWar;
using System;

namespace StageBuilder
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var serviceCollection = new ServiceCollection();

            CompositionRoot.AddServices(serviceCollection);
            serviceCollection.AddSingleton<StageBuilder>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            using (var game = serviceProvider.GetService<StageBuilder>())
                game.Run();
        }
    }
}
