using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    public class TileLoaderException : Exception
    {
        public TileLoaderException()
        {
        }

        public TileLoaderException(string message) : base(message)
        {
        }

        public TileLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
