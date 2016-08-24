namespace Shine.Exceptions
{
    using System;

    public class InvalidMovieNameException : Exception
    {
        public InvalidMovieNameException(string message) : 
            base(message) { }
    }
}
