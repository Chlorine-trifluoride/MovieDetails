using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieLib.Tests
{
    [TestClass()]
    public class MovieControllerTests
    {
        [TestMethod()]
        public void A1ClearCacheTest()  // This has to be executed first
        {
            new MovieController().ClearCache();
        }

        [TestMethod()]
        public async Task GetCachedMoviesTest()
        {
            MovieController movieController = new MovieController();
            List<Movie> movies = await movieController.GetCachedMovies();

            // Assuming the json file does not exists, this should reutrn null
            Assert.IsNull(movies);
        }

        [TestMethod()]
        public async Task AddCachedMovieTest()
        {
            MovieController movieController = new MovieController();

            Movie testMovie1 = new Movie
            {
                Title = "test"
            };

            Movie testMovie2 = new Movie
            {
                Title = "test"
            };

            // Should return true for the first time the same movie is added, false the 2nd time
            Assert.IsTrue(await movieController.AddCachedMovie(testMovie1));
            Assert.IsFalse(await movieController.AddCachedMovie(testMovie2));

            // Clear the test cache
            A1ClearCacheTest();
        }
    }
}