using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace MovieCLI
{
    class MovieSearch
    {
        private MovieController movieController;

        public MovieSearch(MovieController movieController)
        {
            this.movieController = movieController;
        }

        public void Run()
        {
            PrintInstructions();
            string searchString = Console.ReadLine();

            if (!SearchForMovieLocal(searchString)!)
            {
                Console.WriteLine("Movie not found in cache, starting a OMDB search");
                SearchForMovieRemote(searchString);
            }
        }

        private void PrintInstructions()
        {
            Console.Clear();
            Console.WriteLine("Search for a movies by Title");
        }

        private bool SearchForMovieLocal(string searchString)
        {
            searchString = searchString.ToLower();  // Ignore case

            List<Movie> cachedMovies = movieController.GetCachedMovies();

            // Search for exact match
            var movieQuery = from mov in cachedMovies
                    where mov.Title.ToLower() == searchString
                    select mov;

            if (movieQuery.Count() > 0)
            {
                PrintMovieInfo(movieQuery.First(), true);
                return true;
            }

            // Search for partial matches
            movieQuery = from mov in cachedMovies
                             where mov.Title.ToLower().Contains(searchString)
                             select mov;

            if (movieQuery.Count() > 0)
            {
                foreach (Movie movie in movieQuery)
                {
                    Console.WriteLine($"Found {movieQuery.Count()} partial matches");
                    PrintMovieInfo(movie, true);
                    Console.WriteLine("-----");
                    return true;
                }
            }

            return false;
        }

        private void SearchForMovieRemote(string searchString)
        {
            List<Movie> movies = OMDbAPI.SearchForMoviesByTitle(searchString);

            for (int i = 0; i < movies.Count; i++)
            {

                Console.WriteLine($"{i}\t{movies[i].TitleYear}");
            }

            Console.WriteLine("============");
            Console.WriteLine("Enter the ID of the movie you wish to load full info for.");

            // Select the specific movie and get the info
            string input = Console.ReadLine();
            if (int.TryParse(input, out int selection)
                && selection >= 0 && selection < movies.Count)
            {
                    LoadInfoForMovie(movies[selection]);
            }

            else
                Console.WriteLine("Invalid input");
        }

        private void LoadInfoForMovie(Movie movie)
        {
            // Load movie from OMDB
            movie = OMDbAPI.GetMovieByImdbID(movie.IMDBID);

            // Add movie to local cache
            bool addedToCache = false;
            addedToCache = movieController.AddCachedMovie(movie);

            PrintMovieInfo(movie, addedToCache);
        }

        private void PrintMovieInfo(Movie movie, bool foundInCache = false)
        {
            if (foundInCache)
                Console.WriteLine("== Movie found in local cache ==");

            else
                Console.WriteLine("== Movie added to local cache ==");

            PrintAllFieldsUsingReflection(movie);

            Console.WriteLine("=========");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void PrintAllFieldsUsingReflection(Movie movie)
        {
            PropertyInfo[] properties = movie.GetType().GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                Console.WriteLine($"{prop.Name}: {prop.GetValue(movie)?.ToString()}");
            }
        }
    }
}
