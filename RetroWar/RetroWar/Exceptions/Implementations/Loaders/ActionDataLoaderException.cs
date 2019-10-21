using System;

namespace RetroWar.Exceptions.Implementations.Loaders
{
    public class ActionDataLoaderException : Exception
    {
        public ActionDataLoaderException()
        {
        }

        public ActionDataLoaderException(string message) : base(message)
        {
        }

        public ActionDataLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
