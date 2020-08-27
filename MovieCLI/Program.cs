using System;
using System.Collections.Generic;
using MovieLib;

namespace MovieCLI
{
    class Program
    {
        private static bool quit = false;
        private static MovieController movieController;

        static void Main(string[] args)
        {
            movieController = new MovieController();

            while (!quit)
                PrintMenu();
        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("1) Search for a movie");
            Console.WriteLine("2) Print Cached Movie List");
            Console.WriteLine("3) Add Dummy movies");
            Console.WriteLine("Q) Quit program");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        SearchForMovie();
                        return;

                    case ConsoleKey.D2:
                        AddPrintDummyMovies();
                        return;

                    case ConsoleKey.D3:
                        AddDummyMovies();
                        return;

                    case ConsoleKey.Q:
                    case ConsoleKey.E:
                        quit = true;
                        return;
                }
            }
        }

        static void SearchForMovie()
        {
            MovieSearch movieSearch = new MovieSearch(movieController);
            movieSearch.Run();
        }

        static void AddDummyMovies()
        {
            movieController.AddCachedDummyMovies();
        }

        static void AddPrintDummyMovies()
        {
            Console.Clear();

            List<Movie> cachedMovies = movieController.GetCachedMovies();

            for (int i = 0; i < cachedMovies.Count; i++)
            {
                Console.WriteLine(cachedMovies[i].IDTitle);
            }

            Console.WriteLine("Press any key to...");
            Console.ReadKey(true);
        }
    }
}
