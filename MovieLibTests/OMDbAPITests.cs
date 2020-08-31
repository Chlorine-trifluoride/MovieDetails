using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieLib.Tests
{
    [TestClass()]
    public class OMDbAPITests
    {
        [TestMethod()]
        public async Task TestConnectionTest()
        {
            Assert.IsTrue(await OMDbAPI.TestConnection(), "Testing connection to OMDB failed");
        }

        [TestMethod()]
        public async Task SearchForMoviesByTitleTest()
        {
            List<Movie> result = await OMDbAPI.SearchForMoviesByTitle(null);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task GetMovieByImdbIDTest()
        {
            Assert.IsNull(await OMDbAPI.GetMovieByImdbID(null));
        }
    }
}