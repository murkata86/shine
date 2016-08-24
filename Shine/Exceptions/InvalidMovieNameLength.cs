namespace Shine.Exceptions
{
    using System;

    public class InvalidMovieNameLength : Exception
    {
        public InvalidMovieNameLength(string message) : 
            base(message) { }
    }
}
