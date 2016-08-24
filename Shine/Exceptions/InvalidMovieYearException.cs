namespace Shine.Exceptions
{
    using System;

    class InvalidMovieYearException : Exception
    {
        public InvalidMovieYearException(string message) :
            base(message) { }
    }
}
