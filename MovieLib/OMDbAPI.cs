using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieLib
{
    public class OMDbAPI
    {
        private const string TEST_URL = @"https://www.omdbapi.com";
        private const string API_URL = @"https://www.omdbapi.com/?apikey=";
        private const string API_KEY = "b40e1a65"; // please do not abuse my api key

        private static string searchByTitleURL => $"{API_URL}{API_KEY}&t=";
        private static string searchForTitlesURL => $"{API_URL}{API_KEY}&s=";
        private static string searchByIMDB => $"{API_URL}{API_KEY}&i=";


        // This resource can be re-used
        private static SafeHttpClient httpClient = new SafeHttpClient();

        /// <summary>
        /// Test connection, is a firewall blocking us?
        /// SafeHttpClient calls Logger.LogConnectionException(e) if any exceptions are raised
        /// </summary>
        /// <returns>bool successful connection</returns>
        public static async Task<bool> TestConnection()
        {
            string url = TEST_URL;
            using (var response = await httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return false; // Server error occured

                return true;
            }
        }

        /// <summary>
        /// Get one movie by searching for the title without any sorting
        /// </summary>
        /// <param name="title">Movie Title</param>
        /// <returns>A movie with a matching title that could be the wrong movie</returns>
        public static async Task<Movie> SearchForMovieByTitle(string title)
        {
            Movie movie = null;

            string url = $"{searchByTitleURL}{title}";
            using (var response = await httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return null; // Server error occured

                string apiResponse = await response.Content.ReadAsStringAsync();
                movie = JsonSerializer.Deserialize<Movie>(apiResponse);
            }

            return movie;
        }

        /// <summary>
        ///  Search for a list of movies that match the title
        /// </summary>
        /// <param name="title">Movie Title</param>
        /// <returns>A list of movies</returns>
        public static async Task<List<Movie>> SearchForMoviesByTitle(string title)
        {
            List<Movie> movies = null;

            string url = $"{searchForTitlesURL}{title}";
            using (var response = await httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return null; // Server error occured

                string apiResponse = await response.Content.ReadAsStringAsync();
                movies = JsonSerializer.Deserialize<MovieSearch>(apiResponse).ResultMovies;
            }

            return movies;
        }

        /// <summary>
        ///  Load Movie info by specific imdbID
        /// </summary>
        /// <param name="imdbID">Valid IMDB movie ID</param>
        /// <returns>A movie matching the imdbID</returns>
        public static async Task<Movie> GetMovieByImdbID(string imdbID)
        {
            Movie movie = null;

            string url = $"{searchByIMDB}{imdbID}";
            using (var response = await httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return null; // Server error occured

                string apiResponse = await response.Content.ReadAsStringAsync();
                movie = JsonSerializer.Deserialize<Movie>(apiResponse);

                if (movie.Title is null)    // the json did not contain a movie
                    return null;            // possibly invalid IMDB ID
            }

            return movie;
        }
    }
}
