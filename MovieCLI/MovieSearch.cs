using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;

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
            SearchForMovie();
        }

        private void PrintInstructions()
        {
            Console.Clear();
            Console.WriteLine("Search for a movies by Title");
        }

        private void SearchForMovie()
        {
            string searchString = Console.ReadLine();
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
            movie = OMDbAPI.GetMovieByImdbID(movie.IMDBID);
            Console.WriteLine(movie.TitleYear);
            Console.WriteLine(movie.Plot);
            Console.WriteLine(movie.Metascore);

            Console.WriteLine("=========");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
