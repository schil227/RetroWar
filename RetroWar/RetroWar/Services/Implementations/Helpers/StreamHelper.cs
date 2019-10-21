using RetroWar.Services.Interfaces.Helpers;
using System.IO;

namespace RetroWar.Services.Implementations.Helpers
{
    internal class StreamHelper : IStreamHelper
    {
        public string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
