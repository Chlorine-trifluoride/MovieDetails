using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLib.Tests
{
    [TestClass()]
    public class OMDbAPITests
    {
        [TestMethod()]
        public void TestConnectionTest()
        {
            Assert.IsTrue(OMDbAPI.TestConnection(), "Testing connection to OMDB failed");
        }

        [TestMethod()]
        public void SearchForMoviesByTitleTest()
        {
            List<Movie> result = OMDbAPI.SearchForMoviesByTitle(null);
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMovieByImdbIDTest()
        {
            Assert.IsNull(OMDbAPI.GetMovieByImdbID(null));
        }
    }
}