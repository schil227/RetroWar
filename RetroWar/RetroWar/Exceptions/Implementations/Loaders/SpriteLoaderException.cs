using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    internal class SpriteLoaderException : Exception
    {
        public SpriteLoaderException()
        {
        }

        public SpriteLoaderException(string message) : base(message)
        {
        }

        public SpriteLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
