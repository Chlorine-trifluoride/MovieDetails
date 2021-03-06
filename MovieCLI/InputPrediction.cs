﻿using MovieLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCLI
{
    class InputPrediction
    {
        private static InputPrediction inst = new InputPrediction();
        private InputPrediction() { }

        private const string MOVIE_DATASET_PATH_EN = "Data/MovieNames.txt";
        private const string MOVIE_DATASET_PATH_FI = "Data/MoviesFI.txt";
        private const string ALLOWED_CHARS_PATH = "Data/distinct.txt";
        private string[] movieList = null;
        private char[] allowedChars = null;

        private static int cursorDefaultX, cursorDefaultY;

        public static async Task<string> GetUserInput()
        {
            // Load movie list if not already loaded
            Task loadMoviesTask = inst.LoadMovieLists();
            Task loadCharsTask = inst.LoadAllowedCharacters();

            // Await all load tasks
            await Task.WhenAll(loadMoviesTask, loadCharsTask);

            // Save our starting cursor position
            cursorDefaultX = Console.CursorLeft;
            cursorDefaultY = Console.CursorTop;

            // Custom input handling
            return inst.GetCustomInput();
        }

        private async Task LoadMovieLists()
        {
            if (!(movieList is null))   // Already loaded movie list to memory
                return;

            Task<List<string>> enMoviesTask = LoadMovieList(MOVIE_DATASET_PATH_EN);
            Task<List<string>> fiMoviesTask = LoadMovieList(MOVIE_DATASET_PATH_FI);

            // await loading of the movie datasets
            await Task.WhenAll(enMoviesTask, fiMoviesTask);
            movieList = enMoviesTask.Result.Concat(fiMoviesTask.Result).ToArray();

            Logger.Info("MOVIE", $"Loaded total of {movieList.Length} movie names");
        }

        private async Task<List<string>> LoadMovieList(string path)
        {
            Logger.Info("MOVIE", $"Loading movie list from {path} ...");

            try
            {
                return (await File.ReadAllLinesAsync(path)).ToList();
            }
            catch (Exception e)
            {
                Logger.Error("MOVIE", $"Unable to load movie name dataset. Exception: {e.Message}");
            }

            return null;
        }

        private async Task LoadAllowedCharacters()
        {
            if (!(allowedChars is null))    // Already loaded
                return;

            Logger.Info("MOVIE", $"Loading allowed input chars from {ALLOWED_CHARS_PATH} ...");

            try
            {
                // TODO: A bit hacky
                allowedChars = (await File.ReadAllLinesAsync(ALLOWED_CHARS_PATH))
                    .Where(s => s.Length == 1)  // foreign language characters don't fit in a char
                    .Select(s => char.Parse(s)).ToArray();
            }
            catch (Exception e)
            {
                Logger.Error("MOVIE", $"Unable to load allowed input chars. Exception: {e.Message}");
                throw e;
            }
        }

        private string GetCustomInput()
        {
            string input = "";

            for (; ; )
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                char keyChar = keyInfo.KeyChar;
                ConsoleKey key = keyInfo.Key;

                // Handle special keys
                if (key == ConsoleKey.Backspace)
                {
                    if (input != string.Empty)
                        input = input.Remove(input.Length - 1);
                }

                else if (key == ConsoleKey.Spacebar)
                {
                    input += ' ';
                }

                else if (key == ConsoleKey.Tab)
                {
                    input = GetTextPrediction(input);
                }

                else if (key == ConsoleKey.Enter)
                {
                    // Reset console color
                    Console.ForegroundColor = ConsoleColor.White;
                    return input;
                }

                else if (key == ConsoleKey.Escape)
                {
                    // Reset console color
                    Console.ForegroundColor = ConsoleColor.White;
                    return string.Empty;
                }

                else if (!allowedChars.Contains(keyChar))
                {
                    // DO nothing with other special characters
                }

                // Else add the characters to our input string
                else
                {
                    input += keyChar;
                }

                RenderTextPrediction(input, GetTextPrediction(input));
            }
        }

        private string GetTextPrediction(string input)
        {
            var movieQuery = from movie in movieList
                             where movie.ToLower().StripDiacritics().StartsWith(input.StripDiacritics().ToLower())
                             select movie;

            if (movieQuery.Count() < 1)
                return string.Empty;

            return movieQuery.First();
        }

        private void RenderTextPrediction(string input, string prediction)
        {
            Console.SetCursorPosition(cursorDefaultX, cursorDefaultY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(input);   // User typed characters

            if (prediction != string.Empty && input != string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string predictEnd = prediction.Substring(input.Length);
                Console.Write(predictEnd);
            }

            ClearRestOfTheLine();
        }

        private void ClearRestOfTheLine()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            char[] chars = Enumerable.Repeat(' ', Console.BufferWidth - Console.CursorLeft - 1).ToArray();
            Console.Write(chars);

            Console.SetCursorPosition(x, y);
        }
    }
}
