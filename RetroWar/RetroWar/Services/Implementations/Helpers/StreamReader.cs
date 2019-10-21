using RetroWar.Services.Interfaces.Helpers;

namespace RetroWar.Services.Implementations.Helpers
{
    internal class StreamReader : IStreamReader
    {
        public string ReadFile(string fileName)
        {
            using (var reader = new System.IO.StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
