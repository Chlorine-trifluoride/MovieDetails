using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieLib
{
    public class MovieSearch
    {
        [JsonPropertyName("Search")]
        public List<Movie> ResultMovies { get; set; }
    }
}
