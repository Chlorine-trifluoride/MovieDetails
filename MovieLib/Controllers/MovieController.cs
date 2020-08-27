using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MovieLib
{
    public class MovieController
    {
        private const string DB_PATH = @"MovieCache.json";
        private List<Movie> cachedMovies = new List<Movie>();

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
            EnsureDBCacheFileExists();

            string jsonString = "";

            using (StreamReader reader = new StreamReader(DB_PATH))
            {   // Read the whole cache file to memory
                jsonString = reader.ReadToEnd();
            }

            // Deseriliaze the string to a list object in memory

            try
            {
                cachedMovies = JsonSerializer.Deserialize<List<Movie>>(jsonString);
            }
            catch (Exception e)
            {
                // TODO: Add a debugger
                Console.WriteLine($"Invalid JSON: {e.Message}");
            }

            return cachedMovies;
        }

        public void AddCachedMovie(Movie movie)
        {
            if (cachedMovies.Count < 1) // Maybe we haven't loaded the DB yet?
                GetCachedMovies();

            movie.ID = cachedMovies.Count;  // add appropriate ID to our movie
            cachedMovies.Add(movie);        // Add our movie to memory cache

            // Serialize the whole cache to json
            string jsonString = JsonSerializer.Serialize<List<Movie>>(cachedMovies);

            using (StreamWriter writer = new StreamWriter(DB_PATH))
            {   // Write the cache to file
                writer.Write(jsonString);
            }
        }

        /// <summary> Creates and adds a Movie object with minimal info
        public void AddCachedMovie(string title, string plot, string runtime)
        {
            Movie movie = new Movie
            {
                Title = title,
                Plot = plot,
                Runtime = runtime
            };

            AddCachedMovie(movie);
        }

        public void AddCachedDummyMovies()
        {
            AddCachedMovie("Demo", "Things happen", "120 minutes");
            AddCachedMovie("Test", "Lorem ipsum", "60 minutes");
        }
    }
}
