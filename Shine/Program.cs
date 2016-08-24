namespace Shine
{
    using System;
    using Shine.Models;

    public class Program
    {
        public static void Main()
        {
            MovieScrapper ms = new MovieScrapper("Exodus");
            ms.GetMovieName();
            ms.GetReleaseDate();
            ms.GetMovieGenres();
            ms.GetPlot();
            ms.GetRuntime();
            ms.GetCountries();
            ms.GetRating();
            ms.GetLanguages();
            ms.GetBudget();
            ms.GetRevenue();
            ms.GetPoster();
            ms.GetBackdrops();

            string path = Console.ReadLine();

            FileSystemChecker checker = new FileSystemChecker(path);
            checker.Run();

            
        }
    }
}
