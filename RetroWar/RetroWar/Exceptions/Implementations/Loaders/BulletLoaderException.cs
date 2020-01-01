using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    public class BulletLoaderException : Exception
    {
        public BulletLoaderException()
        {
        }

        public BulletLoaderException(string message) : base(message)
        {
        }

        public BulletLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
