using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    public class StageLoaderException : Exception
    {
        public StageLoaderException()
        {
        }

        public StageLoaderException(string message) : base(message)
        {
        }

        public StageLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
