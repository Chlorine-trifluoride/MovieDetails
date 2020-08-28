using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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


        /// <summary>
        /// Test connection, is a firewall blocking us?
        /// </summary>
        /// <returns>bool success and any thrown exception</returns>
        public static (bool, Exception) TestConnection()
        {
            using (SafeHttpClient httpClient = new SafeHttpClient())
            {
                string url = TEST_URL;
                using (var response = httpClient.GetAsync(url).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        return (false, null); // Server error occured

                    return (true, null);
                }
            }
        }

        /// <summary>
        /// Get one movie by searching for the title without any sorting
        /// </summary>
        /// <param name="title">Movie Title</param>
        /// <returns>A movie with a matching title that could be the wrong movie</returns>
        public static Movie SearchForMovieByTitle(string title)
        {
            Movie movie = null;

            using (SafeHttpClient httpClient = new SafeHttpClient())
            {
                string url = $"{searchByTitleURL}{title}";
                using (var response = httpClient.GetAsync(url).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        return null; // Server error occured

                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    movie = JsonSerializer.Deserialize<Movie>(apiResponse);
                }
            }

            return movie;
        }

        /// <summary>
        ///  Search for a list of movies that match the title
        /// </summary>
        /// <param name="title">Movie Title</param>
        /// <returns>A list of movies</returns>
        public static List<Movie> SearchForMoviesByTitle(string title)
        {
            List<Movie> movies = null;

            using (SafeHttpClient httpClient = new SafeHttpClient())
            {
                string url = $"{searchForTitlesURL}{title}";
                using (var response = httpClient.GetAsync(url).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        return null; // Server error occured

                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    movies = JsonSerializer.Deserialize<MovieSearch>(apiResponse).ResultMovies;
                }
            }

            return movies;
        }

        /// <summary>
        ///  Load Movie info by specific imdbID
        /// </summary>
        /// <param name="imdbID">Valid IMDB movie ID</param>
        /// <returns>A movie matching the imdbID</returns>
        public static Movie GetMovieByImdbID(string imdbID)
        {
            Movie movie = null;

            using (SafeHttpClient httpClient = new SafeHttpClient())
            {
                string url = $"{searchByIMDB}{imdbID}";
                using (var response = httpClient.GetAsync(url).Result)
                {
                    if (!response.IsSuccessStatusCode)
                        return null; // Server error occured

                    string apiResponse = response.Content.ReadAsStringAsync().Result;
                    movie = JsonSerializer.Deserialize<Movie>(apiResponse);
                }
            }

            return movie;
        }
    }
}
