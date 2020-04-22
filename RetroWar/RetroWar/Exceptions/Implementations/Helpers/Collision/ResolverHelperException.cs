using System;

namespace RetroWar.Exceptions.Implementations.Helpers.Collision
{
    public class ResolverHelperException : Exception
    {
        public ResolverHelperException()
        {
        }

        public ResolverHelperException(string message) : base(message)
        {
        }

        public ResolverHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
