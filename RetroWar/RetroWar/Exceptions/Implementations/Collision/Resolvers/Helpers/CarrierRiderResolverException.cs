using System;

namespace RetroWar.Exceptions.Implementations.Collision.Resolvers.Helpers
{
    public class CarrierRiderResolverException : Exception
    {
        public CarrierRiderResolverException()
        {
        }

        public CarrierRiderResolverException(string message) : base(message)
        {
        }

        public CarrierRiderResolverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
