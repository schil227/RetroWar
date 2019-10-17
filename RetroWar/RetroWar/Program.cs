using Microsoft.Extensions.DependencyInjection;
using System;

namespace RetroWar
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

            var serviceProvider = serviceCollection.BuildServiceProvider();

            using (var game = serviceProvider.GetService<MainGame>())
                game.Run();
        }
    }
}
