using System;

namespace RetroWar.Exceptions.Implementations.Collision
{
    public class CollisionFinderException : Exception
    {
        public CollisionFinderException()
        {
        }

        public CollisionFinderException(string message) : base(message)
        {
        }

        public CollisionFinderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
