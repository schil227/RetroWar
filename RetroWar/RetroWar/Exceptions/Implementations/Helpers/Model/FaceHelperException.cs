using System;

namespace RetroWar.Exceptions.Implementations.Helpers.Model
{
    public class FaceHelperException : Exception
    {
        public FaceHelperException()
        {
        }

        public FaceHelperException(string message) : base(message)
        {
        }

        public FaceHelperException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
