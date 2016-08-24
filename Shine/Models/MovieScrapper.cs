namespace Shine.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;
    using Shine.Exceptions;
    using Shine.Utils;

    public class MovieScrapper
    {
        private string imdbId;
        private string movieName;

        private int movieYear;

        private JObject mainInfoData;
        private JObject configurationData;


        public MovieScrapper(string movieName) : 
            this(movieName, -1) { }

        public MovieScrapper(string movieName, int movieYear)
        {
            this.MovieName = movieName;
            this.MovieYear = movieYear;
            this.SetImdbId();
            this.SetMainInfoData(this.imdbId);
            this.SetConfigurationData(this.imdbId);
        }

        public string MovieName
        {
            get
            {
                return this.movieName;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidMovieNameException(Messages.InvalidMovieName);
                }

                if (value.Length < Constants.MinMovieNameLength || value.Length > Constants.MaxMovieNameLength)
                {
                    throw new InvalidMovieNameLength(string.Format(Messages.InvalidMovieNameLength, Constants.MinMovieNameLength, 
                        Constants.MaxMovieNameLength));
                }

                this.movieName = value;
            }
        }

        public int MovieYear
        {
            get
            {
                return this.movieYear;
            }

            private set
            {
                if ((value < Constants.MinMovieYearAllowed || value > Constants.MaxMovieYearAllowed) && value != -1)
                {
                    throw new InvalidMovieYearException(string.Format(Messages.InvalidMovieYear, 
                        Constants.MinMovieYearAllowed, Constants.MaxMovieYearAllowed));
                }

                this.movieYear = value;
            }
        }

        private void SetImdbId()
        {
            string result = string.Empty;

            if (this.MovieYear == -1)
            {
                result = this.GetFileContents(Constants.imdbURL + this.MovieName);
            }
            else
            {
                result = this.GetFileContents(Constants.imdbURL + this.MovieName + " (" + this.MovieYear + ")");
            }

            string pattern = "<td class=\"result_text\">.*?href=\"/title/(?'id'(.*?))/.*?</td>";

            Regex reg = new Regex(pattern);

            Match match = reg.Match(result);
            
            if (match.Success)
            {
                this.imdbId = match.Groups["id"].ToString();
            }
        }

        private void SetMainInfoData(string imdbId)
        {
            string url = string.Format(Constants.baseURL + "movie/" + this.imdbId + "?append_to_response=credits,releases,images&api_key=" + Constants.apiKey);

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                var obj = JObject.Parse(json);

                this.mainInfoData = obj;
            }
        }

        private void SetConfigurationData(string imdbId)
        {
            string url = string.Format(Constants.baseURL + "configuration?api_key=" + Constants.apiKey);

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                var obj = JObject.Parse(json);

                this.configurationData = obj;
            }
        }

        private string GetFileContents(string fileName)
        {
            string sContents = string.Empty;
            if (fileName.ToLower().IndexOf("http:") > -1)
            {
                // URL 
                WebClient wc = new WebClient();
                byte[] response = wc.DownloadData(fileName);
                sContents = Encoding.ASCII.GetString(response);
            }
            else {
                // Regular Filename 
                StreamReader sr = new StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }

        public void GetMovieName()
        {
            var name = (string)this.mainInfoData.SelectToken("original_title");
            Console.WriteLine(name);
        }

        public void GetReleaseDate()
        {
            var year = (string) this.mainInfoData.SelectToken("release_date");
            Console.WriteLine(year);
        }

        public void GetMovieGenres()
        {
            var genres = this.mainInfoData.SelectToken("genres").ToArray();

            foreach (var genre in genres)
            {
                var name = (string) genre.SelectToken("name");
                Console.WriteLine(name);
            }
        }

        public void GetPlot()
        {
            var plot = this.mainInfoData.SelectToken("overview");

            Console.WriteLine(plot);
        }

        public void GetRuntime()
        {
            var runtime = this.mainInfoData.SelectToken("runtime");

            Console.WriteLine(runtime);
        }

        public void GetCountries()
        {
            var countries = this.mainInfoData.SelectToken("production_countries");

            foreach (var country in countries)
            {
                var countryName = country.SelectToken("name");
                Console.WriteLine(countryName);
            }
        }

        public void GetRating()
        {
            var rating = this.mainInfoData.SelectToken("vote_average");

            Console.WriteLine(rating);
        }

        public void GetLanguages()
        {
            var language = this.mainInfoData.SelectToken("original_language");

            Console.WriteLine(language.ToString().ToUpper());

            //foreach (var language in languages)
            //{
            //    var languageName = language.SelectToken("name");
            //    Console.WriteLine(languageName);
            //}
        }

        public void GetBudget()
        {
            var budget = this.mainInfoData.SelectToken("budget");

            Console.WriteLine("$" + budget);
        }

        public void GetRevenue()
        {
            var revenue = this.mainInfoData.SelectToken("revenue");

            Console.WriteLine("$" + revenue);
        }

        public void GetPoster()
        {
            var url = (string)this.configurationData.SelectToken("images.base_url");
            var size = this.configurationData.SelectToken("images.poster_sizes")[2];
            var image = (string)this.mainInfoData.SelectToken("poster_path");

            string posterPath = string.Format(url + size + image);

            Console.WriteLine(posterPath);
        }

        public void GetBackdrops()
        {
            List<string> imageCollection = new List<string>();

            var images = this.mainInfoData.SelectToken("images.backdrops");

            var url = (string)this.configurationData.SelectToken("images.base_url") + 
                (string)(this.configurationData.SelectToken("images.backdrop_sizes")[0]);

            foreach (var image in images)
            {
                var imagePath = url + image.SelectToken("file_path");

                imageCollection.Add(imagePath);
            }

            Console.WriteLine(string.Join("\r\n", imageCollection));
        }
    }
}
