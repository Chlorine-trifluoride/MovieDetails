using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MovieCLI
{
    class MovieSearch
    {
        private MovieController movieController;

        public MovieSearch(MovieController movieController)
        {
            this.movieController = movieController;
        }

        public async Task Run()
        {
            await DoMovieSearch();
        }

        private async Task DoMovieSearch(bool includeLocal = true)
        {
            PrintInstructions();
            string searchString = await InputPrediction.GetUserInput();

            if (searchString == string.Empty)
                return;

            List<Movie> foundMovies = null;

            if (includeLocal) // Search local cache?
            {
                foundMovies = await SearchForMoviesLocal(searchString);
                Logger.Info("CACHE", $"Found {foundMovies?.Count ?? 0} matches in local cache for {searchString}");
            }

            if (foundMovies is null)
            {
                Console.WriteLine("Movie not found in cache, starting a OMDB search");
                foundMovies = await SearchForMovieRemote(searchString);
            }

            if (foundMovies is null)
            {   // We didnt find any locally or in OMDB
                Console.WriteLine("No movies matching your search string were found =(");
                Console.WriteLine("Press any key to...");
                Console.ReadKey(true);
            }

            // Movies found, present user with a selection screen
            if (foundMovies?.Count() > 0)
                await PresentUserWithMovieOptions(foundMovies);
        }

        private bool PresentYesNoQuestion(string question)
        {
            Console.ForegroundColor = ConsoleColor.Red;     // Highlight the yes no question
            Console.WriteLine($"{question} Y/N?");
            Console.ForegroundColor = ConsoleColor.White;   // Restore text color

            if (Console.ReadKey(true).Key == ConsoleKey.Y)
                return true;

            return false;
        }

        private async Task PresentUserWithMovieOptions(List<Movie> movies)
        {
            for (int i = 0; i < movies.Count; i++)
            {
                Console.WriteLine($"{i}\t{movies[i].TitleYear}");
            }

            if (!PresentYesNoQuestion("Is your movie in this list?"))
            {
                if (PresentYesNoQuestion("Do you want to search OMDB for a movie title?"))
                    await DoMovieSearch(false); // Only search OMDB

                return;
            }

            Console.WriteLine("============");
            Console.WriteLine("Enter the ID of the movie you wish to show full info for.");

            // Select the specific movie and get the info
            string input = Console.ReadLine();
            if (int.TryParse(input, out int selection)
                && selection >= 0 && selection < movies.Count)
            {
                PrintMovieInfo(movies[selection]);
            }

            else
                Console.WriteLine("Invalid input");
        }

        private void PrintInstructions()
        {
            Console.Clear();
            Console.WriteLine("Search for a movies by Title");
            Console.WriteLine("Press TAB to auto complete | ESC to return to main menu");
        }

        private async Task<List<Movie>> SearchForMoviesLocal(string searchString)
        {
            searchString = searchString.ToLower().StripDiacritics();  // Ignore case

            var cacheTask = movieController.GetCachedMovies();
            Logger.Info("SEARCH", $"Searching for {searchString} in local cache");
            List<Movie> cachedMovies = await cacheTask;

            if (cachedMovies is null)
                return null;

            // Search for exact match
            var movieQuery = from mov in cachedMovies
                    where mov.Title.ToLower().StripDiacritics() == searchString
                    select mov;

            if (movieQuery.Count() > 0)
            {
                return movieQuery.ToList();
            }

            // Search for partial matches
            movieQuery = from mov in cachedMovies
                             where mov.Title.ToLower().StripDiacritics().Contains(searchString)
                             select mov;

            if (movieQuery.Count() > 0)
            {
                Console.WriteLine($"Found {movieQuery.Count()} partial match(es)");
                return movieQuery.ToList();
            }

            return null;
        }

        private async Task<List<Movie>> SearchForMovieRemote(string searchString)
        {
            var searchTask = OMDbAPI.SearchForMoviesByTitle(searchString);
            Logger.Info("SEARCH", $"Searching for {searchString} in OMDB");
            List<Movie> movies = await searchTask;
            

            if (movies is null) // no movies found
            {
                Logger.Warn("HTTP", $"Found 0 matches in OMDB for {searchString}");
                return null;
            }

            Logger.Info("SEARCH", $"Found {movies?.Count ?? 0} matches in OMDB for {searchString}");

            for (int i = 0; i < movies.Count; i++)
            {
                Console.WriteLine($"{i}\t{movies[i].TitleYear}");
            }

            if (!PresentYesNoQuestion("Is your movie in this list?"))
            {
                return null;
            }

            Console.WriteLine("============");
            Console.WriteLine("Enter the ID of the movie you wish to load full info for.");

            // Select the specific movie and get the info
            string input = Console.ReadLine();
            if (int.TryParse(input, out int selection)
                && selection >= 0 && selection < movies.Count)
            {
                Movie m = await LoadInfoForMovie(movies[selection]);  // Load the Movie from OMDB
                await movieController.AddCachedMovie(m);              // Add Movie to local cache
                Console.WriteLine($"{m.TitleYear} added to local cache");
                List<Movie> output = new List<Movie>();         // Create a new list to return
                output.Add(m);                                  // add single movie to the list
                return output;                                  // return the list with a single movie element
            }

            else
                Console.WriteLine("Invalid input");

            return null;
        }

        private async Task<Movie> LoadInfoForMovie(Movie movie)
        {
            // Load movie from OMDB
            var getInfoTask = OMDbAPI.GetMovieByImdbID(movie.IMDBID);
            Logger.Info("SEARCH", $"Downloading movie details for {movie.IDTitle} from OMDB");
            return await getInfoTask;
        }

        private void PrintMovieInfo(Movie movie)
        {
            PrintAllFieldsUsingReflection(movie);

            Console.WriteLine("=========");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
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
