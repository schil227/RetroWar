using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    public class IllusionLoaderException : Exception
    {
        public IllusionLoaderException()
        {
        }

        public IllusionLoaderException(string message) : base(message)
        {
        }

        public IllusionLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
