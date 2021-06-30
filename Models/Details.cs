using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Details
    {
        public int Id { get; set; }
        public string Overview { get; set; }
        public Decimal Vote_Average { get; set; }
        public string Title { get; set; }
        public DateTime Release_Date { get; set; }
        public string Poster_Path { get; set; }
    }
}