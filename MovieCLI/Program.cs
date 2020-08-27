using System;
using System.Collections.Generic;
using MovieLib;

namespace MovieCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieController movieController = new MovieController();
            movieController.AddCachedDummyMovies();

            List<Movie> cachedMovies = movieController.GetCachedMovies();

            for (int i = 0; i < cachedMovies.Count; i++)
            {
                Console.WriteLine(cachedMovies[i].IDTitle);
            }
        }
    }
}
