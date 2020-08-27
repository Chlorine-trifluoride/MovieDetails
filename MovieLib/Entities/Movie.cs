using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieLib
{
    public class Movie
    {
        #region MODEL
        [Key]
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Year")]
        public string Year { get; set; }

        [JsonPropertyName("Rated")]
        public string Rated { get; set; }

        [JsonPropertyName("Runtime")]
        public string Runtime { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; }

        [JsonPropertyName("Director")]
        public string Director { get; set; }

        [JsonPropertyName("Plot")]
        public string Plot { get; set; }

        [JsonPropertyName("Poster")]
        public string PosterURL { get; set; }

        // ratings?

        [JsonPropertyName("Metascore")]
        public string Metascore { get; set; }

        [JsonPropertyName("imdbID")]
        public string IMDBID { get; set; }

        [JsonPropertyName("BoxOffice")]
        public string BoxOffice { get; set; }

        [JsonPropertyName("Response")]
        public string Response { get; set; }
        #endregion

        [JsonIgnore]
        public string TitleYear => $"{Title} ({Year})";

        [JsonIgnore]
        public string IDTitle => $"{ID}: {Title}";
    }
}
