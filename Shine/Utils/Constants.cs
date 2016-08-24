namespace Shine.Utils
{
    using System;

    public static class Constants
    {
        public const string baseURL = @"http://api.themoviedb.org/3/";
        public const string imdbURL = @"http://www.imdb.com/find?s=all&q=";

        public const string apiKey = "8a2494a8d37d731021b5d9452495090e";

        public const int MinMovieNameLength = 2;
        public const int MaxMovieNameLength = 50;

        public static int MinMovieYearAllowed = 1970;
        public static int MaxMovieYearAllowed = DateTime.Now.Year;
    }
}
