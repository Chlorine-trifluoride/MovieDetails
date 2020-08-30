﻿using System;
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
            Console.WriteLine("4) Test OMDB Connection");
            Console.WriteLine("Q) Quit program");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        SearchForMovie();
                        return;

                    case ConsoleKey.D2:
                        PrintMovieCache();
                        return;

                    case ConsoleKey.D3:
                        AddDummyMovies();
                        return;

                    case ConsoleKey.D4:
                        TestConnection();
                        return;

                    case ConsoleKey.Q:
                    case ConsoleKey.E:
                        quit = true;
                        return;
                }
            }
        }

        static void TestConnection()
        {
            bool success = OMDbAPI.TestConnection();

            if (success)
                Console.WriteLine("Connection established");

            else
            {
                Console.WriteLine("Connection failure");
            }

            Console.WriteLine("==== Press any key to... ====");
            Console.ReadKey(true);
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

        static void PrintMovieCache()
        {
            Console.Clear();

            List<Movie> cachedMovies = movieController.GetCachedMovies();

            for (int i = 0; i < cachedMovies.Count; i++)
            {
                Movie m = cachedMovies[i];
                Console.WriteLine($"{m.ID}: {m.TitleYear}");
            }

            Console.WriteLine("Press any key to...");
            Console.ReadKey(true);
        }
    }
}
