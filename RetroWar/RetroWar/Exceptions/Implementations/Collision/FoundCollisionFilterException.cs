using System;

namespace RetroWar.Exceptions.Implementations.Collision
{
    public class FoundCollisionFilterException : Exception
    {
        public FoundCollisionFilterException()
        {
        }

        public FoundCollisionFilterException(string message) : base(message)
        {
        }

        public FoundCollisionFilterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
