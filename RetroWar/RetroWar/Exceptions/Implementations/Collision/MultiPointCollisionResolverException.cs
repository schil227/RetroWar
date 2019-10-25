using System;

namespace RetroWar.Exceptions.Implementations.Collision
{
    public class MultiPointCollisionResolverException : Exception
    {
        public MultiPointCollisionResolverException()
        {
        }

        public MultiPointCollisionResolverException(string message) : base(message)
        {
        }

        public MultiPointCollisionResolverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
