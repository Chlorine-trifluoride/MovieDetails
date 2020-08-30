using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MovieLib
{
    public class MovieController
    {
        private const string DB_PATH = @"MovieCache.json";
        private List<Movie> cachedMovies = new List<Movie>();
        private bool isMemCacheLoaded = false;

        private void EnsureDBCacheFileExists()
        {
            if (!File.Exists(DB_PATH))
            {
                FileStream fs = File.Create(DB_PATH);
                fs.Close();
            }
        }

        public List<Movie> GetCachedMovies()
        {
            if (isMemCacheLoaded) // we already loaded from file
                return cachedMovies;

            EnsureDBCacheFileExists();

            string jsonString = "";

            using (StreamReader reader = new StreamReader(DB_PATH))
            {   // Read the whole cache file to memory
                jsonString = reader.ReadToEnd();
            }

            // Deseriliaze the string to a list object in memory

            try // The JSON file might be empty or corrupted
            {
                cachedMovies = JsonSerializer.Deserialize<List<Movie>>(jsonString);
            }
            catch (Exception e)
            {
                // TODO: Add a debugger
                Console.WriteLine($"Invalid JSON: {e.Message}");
                return null;
            }

            isMemCacheLoaded = true;
            return cachedMovies;
        }

        /// <summary>
        /// Add Movie to local cache
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>Returns true if the movie was added. False if it was not</returns>
        public bool AddCachedMovie(Movie movie)
        {
            if (cachedMovies.Count < 1) // Maybe we haven't loaded the DB yet?
                GetCachedMovies();

            if (IsMovieAlreadyInCache(movie))
                return false; // Don't add the same movie twice

            movie.ID = cachedMovies.Count;  // add appropriate ID to our movie
            cachedMovies.Add(movie);        // Add our movie to memory cache

            // Serialize the whole cache to json
            string jsonString = JsonSerializer.Serialize<List<Movie>>(cachedMovies);

            using (StreamWriter writer = new StreamWriter(DB_PATH))
            {   // Write the cache to file
                writer.Write(jsonString);
            }

            return true;
        }

        /// <summary>
        /// Check if the movie is already in our local cache. The matching is done based on Title and Release Year
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>true if the movie is already in our local cache</returns>
        private bool IsMovieAlreadyInCache(Movie movie)
        {
            if (cachedMovies.Count < 1) // Maybe we haven't loaded the DB yet?
                GetCachedMovies();

            var queryResult = cachedMovies.Where(x => x.Title == movie.Title && x.Year == movie.Year);

            if (queryResult.Count() == 0)
                return false;

            return true;
        }

        /// <summary> Creates and adds a Movie object with minimal info
        public void AddCachedMovie(string title, string year, string plot, string runtime)
        {
            Movie movie = new Movie
            {
                Title = title,
                Year = year,
                Plot = plot,
                Runtime = runtime
            };

            AddCachedMovie(movie);
        }

        public void AddCachedDummyMovies()
        {
            AddCachedMovie("Fake Movie", "1985", "Things happen", "120 minutes");
            AddCachedMovie("Horrible Movie", "2001", "Lorem ipsum", "60 minutes");
        }

        public void ClearCache()
        {
            File.Delete(DB_PATH);
        }
    }
}
