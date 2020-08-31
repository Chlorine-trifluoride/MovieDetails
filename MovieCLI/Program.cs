﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieLib;

namespace MovieCLI
{
    class Program
    {
        private static bool quit = false;
        private static MovieController movieController;

        static async Task Main(string[] args)
        {
            movieController = new MovieController();

            while (!quit)
                await PrintMenu();
        }

        static async Task PrintMenu()
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
                        await SearchForMovie();
                        return;

                    case ConsoleKey.D2:
                        await PrintMovieCache();
                        return;

                    case ConsoleKey.D3:
                        await AddDummyMovies();
                        return;

                    case ConsoleKey.D4:
                        await TestConnection();
                        return;

                    case ConsoleKey.Q:
                    case ConsoleKey.E:
                        quit = true;
                        return;
                }
            }
        }

        static async Task TestConnection()
        {
            bool success = await OMDbAPI.TestConnection();

            if (success)
                Console.WriteLine("Connection established");

            else
            {
                Console.WriteLine("Connection failure");
            }

            Console.WriteLine("==== Press any key to... ====");
            Console.ReadKey(true);
        }

        static async Task SearchForMovie()
        {
            MovieSearch movieSearch = new MovieSearch(movieController);
            await movieSearch.Run();
        }

        static async Task AddDummyMovies()
        {
            await movieController.AddCachedDummyMovies();
        }

        static async Task PrintMovieCache()
        {
            Console.Clear();

            List<Movie> cachedMovies = await movieController.GetCachedMovies();

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
